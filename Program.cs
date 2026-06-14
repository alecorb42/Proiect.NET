using System.Diagnostics;
using Silk.NET.SDL;

namespace TheAdventure;

public class HighScoreStorageException : Exception
{
    public HighScoreStorageException(string message, Exception innerException) : base(message, innerException) { }
}

public static class Program
{
    public static void Main()
    {
        var sdl = new Sdl(new SdlContext());
        var timer = new Stopwatch();
        var sdlEvent = new Event();

        ReadOnlySpan<byte> keyboardState;
        unsafe
        {
            keyboardState = new(sdl.GetKeyboardState(null), (int)KeyCode.Count);
        }

        var sdlInitResult = sdl.Init(Sdl.InitVideo | Sdl.InitAudio | Sdl.InitEvents | Sdl.InitTimer);
        if (sdlInitResult < 0)
        {
            throw new InvalidOperationException("Failed to initialize SDL.");
        }

        IntPtr window;
        unsafe
        {
            window = (IntPtr)sdl.CreateWindow(
                "Space Shooter", Sdl.WindowposUndefined, Sdl.WindowposUndefined, 800, 800,
                (uint)WindowFlags.Resizable | (uint)WindowFlags.AllowHighdpi
            );

            if (window == IntPtr.Zero)
            {
                throw new Exception("Failed to create window.");
            }
        }

        IntPtr renderer;
        unsafe
        {
            renderer = (IntPtr)sdl.CreateRenderer((Window*)window, -1, (uint)RendererFlags.Accelerated);
            sdl.RenderSetVSync((Renderer*)renderer, 1);
        }

        if (renderer == IntPtr.Zero)
        {
            throw new Exception("Failed to create renderer.");
        }

        var lasers = new List<Laser>();
        var asteroids = new List<Asteroid>();
        var random = new Random();
        var player = new PlayerShip(380, 700); 

        bool quit = false;
        int score = 0;
        double asteroidSpawnTimer = 0;
        double spawnInterval = 1.0; 
        
        double timeRemaining = 60.0; 
        int maxBarWidth = 760; 

        int highScore = 0;
        string highScoreFilePath = "highscore.txt";

        // AI-generated 
        try
        {
            if (File.Exists(highScoreFilePath))
            {
                string fileContent = File.ReadAllText(highScoreFilePath);
                int.TryParse(fileContent, out highScore);
            }
            Console.WriteLine($"High score loaded from disk. Current record is {highScore} points.");
        }
        catch (Exception ex)
        {
            throw new HighScoreStorageException("Failed to read high score file from disk.", ex);
        }
        // end AI-generated
        timer.Start();

        while (!quit)
        {
            while (sdl.PollEvent(ref sdlEvent) != 0)
            {
                if (sdlEvent.Type == (uint)EventType.Quit)
                {
                    quit = true;
                    break;
                }

                if (sdlEvent.Type == (uint)EventType.Keydown)
                {
                    var pressedKey = (KeyCode)sdlEvent.Key.Keysym.Scancode;
                    if (pressedKey == KeyCode.Space)
                    {
                        double laserX = player.X + (player.Width / 2) - 2; 
                        double laserY = player.Y; 
                        lasers.Add(new Laser(laserX, laserY));        
                    }
                }
            }

            var elapsed = timer.Elapsed;
            timer.Restart();
            double deltaTime = elapsed.TotalSeconds;

            foreach(var laser in lasers)
            {
                laser.Update(deltaTime);
            }

            if(keyboardState[(byte)KeyCode.Left] > 0 || keyboardState[(byte)KeyCode.A] > 0)
            {
                player.MoveLeft(deltaTime);
            }
            if(keyboardState[(byte)KeyCode.Right] > 0 || keyboardState[(byte)KeyCode.D] > 0)
            {
                player.MoveRight(deltaTime, 800);
            }
   
            asteroidSpawnTimer += deltaTime;
            if (asteroidSpawnTimer >= spawnInterval)
            {
                asteroidSpawnTimer = 0;
                double randomX = random.Next(0, 760);
                double randomY = random.Next(-100, -40); 
                double randomSpeed = random.Next(100, 250); 
                asteroids.Add(new Asteroid(randomX, randomY, randomSpeed));
            }   

            foreach(var asteroid in asteroids)
            {
                asteroid.Update(deltaTime);
            }

            foreach(var laser in lasers)
            {
                foreach(var asteroid in asteroids)
                {
                    if(laser.IsActive && asteroid.IsActive && laser.CheckCollision(asteroid))
                    {
                        laser.IsActive = false;
                        asteroid.IsActive = false;
                        score += 10; 
                    }
                }
            }

            lasers.RemoveAll(l => !l.IsActive);
            asteroids.RemoveAll(a => !a.IsActive);

            if (asteroids.Any(asteroid => asteroid.IsActive && player.CheckCollision(asteroid)))
            {
                quit = true;
                Console.WriteLine("Game over! Your ship was destroyed by an asteroid.");
                break;
            }

            timeRemaining -= deltaTime;
            int currentBarWidth = score * 2;
            
            if (currentBarWidth >= maxBarWidth)
            {
                quit = true; 
                Console.WriteLine("Congratulations! You filled the bar and won the game!");
                break;
            }

            if (timeRemaining <= 0)
            {
                timeRemaining = 0; 
                quit = true;       
                Console.WriteLine("Time's up! Game over!");
                break;
            }

            unsafe
            {
                var r = (Renderer *)renderer;

                sdl.SetRenderDrawColor(r, 0, 0, 0, 255);
                sdl.RenderClear(r);

                player.Render(sdl, r);

                foreach(var laser in lasers)
                {
                    laser.Render(sdl, r);
                }

                foreach(var asteroid in asteroids)
                {
                    asteroid.Render(sdl, r);
                }

                sdl.SetRenderDrawColor(r, 255, 255, 255, 255); 
                
                sdl.SetRenderDrawColor(r, 25, 25, 25, 255); 
                for(int i = 0; i < 35; i++)
                {
                    sdl.RenderDrawLine(r, 0, i, 800, i);
                }

                int timeBarLength = (int)((timeRemaining / 60.0) * maxBarWidth);
                sdl.SetRenderDrawColor(r, 150, 0, 0, 255);
                for(int i = 10; i < 25; i++)
                {
                    sdl.RenderDrawLine(r, 20, i, 20 + timeBarLength, i);
                }

                int barLength = Math.Min((score * 2), maxBarWidth);
                sdl.SetRenderDrawColor(r, 0, 230, 90, 255);
                for(int i = 10; i < 25; i++)
                {
                    if(barLength > 0)
                    {
                        sdl.RenderDrawLine(r, 20, i, 20 + barLength, i);
                    }
                }

                sdl.SetRenderDrawColor(r, 120, 120, 120, 255);
                sdl.RenderDrawLine(r, 0, 35, 800, 35);

                sdl.RenderPresent(r);
            }
        }

        Console.WriteLine($"Your final score is: {score}");
        if (score > highScore)
        {
            try
            {
                File.WriteAllText(highScoreFilePath, score.ToString());
                Console.WriteLine($"NEW RECORD! You beat the old high score of {highScore}. Saved to disk.");
            }
            catch (Exception ex)
            {
                throw new HighScoreStorageException("Could not save the new high score to disk.", ex);
            }
        }
        else
        {
            Console.WriteLine($"The high score to beat remains: {highScore} points.");
        }

        unsafe
        {
            sdl.DestroyWindow((Window*)window);
        }
        sdl.Quit();
    }
}