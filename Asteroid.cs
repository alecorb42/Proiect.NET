using Silk.NET.SDL;

namespace TheAdventure;

public class Asteroid : GameObject
{
    private readonly double _speed;

    public Asteroid(double x, double y, double speed) : base(x, y, 30, 30)
    {
        _speed = speed;
    }

    public override void Update(double deltaTime)
    {
        Y += _speed * deltaTime;
        if (Y > 800)
        {
            IsActive = false;
        }
    }
    // AI-generated
    public override unsafe void Render(Sdl sdl, Renderer* renderer)
    {
        // Matrice 10x10 pentru un asteroid circular/pietros retro (inspirat din imagini)
        // 0 = Transparent, 1 = Contur Negru, 2 = Gri Închis (Corp), 3 = Gri Deschis (Lumină/Cratere)
        int[,] sprite = new int[,] {
            { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 3, 3, 2, 2, 1, 0, 0 },
            { 0, 1, 3, 3, 3, 2, 2, 2, 1, 0 },
            { 1, 3, 3, 1, 1, 2, 2, 2, 2, 1 }, 
            { 1, 3, 2, 1, 1, 2, 2, 3, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 3, 3, 2, 1 },
            { 1, 2, 2, 2, 2, 2, 2, 2, 2, 1 },
            { 0, 1, 2, 2, 3, 2, 2, 2, 1, 0 },
            { 0, 0, 1, 2, 2, 2, 2, 1, 0, 0 },
            { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0 }
        };

        int pixelSize = 3; 

        for (int row = 0; row < sprite.GetLength(0); row++)
        {
            for (int col = 0; col < sprite.GetLength(1); col++)
            {
                int colorCode = sprite[row, col];
                if (colorCode == 0) continue;

                switch (colorCode)
                {
                    case 1: sdl.SetRenderDrawColor(renderer, 30, 30, 30, 255); break;     // Contur Închis
                    case 2: sdl.SetRenderDrawColor(renderer, 110, 110, 110, 255); break; // Gri Piatră
                    case 3: sdl.SetRenderDrawColor(renderer, 170, 170, 170, 255); break; // Gri Deschis / Cratere
                }

                int pixelX = (int)X + (col * pixelSize);
                int pixelY = (int)Y + (row * pixelSize);

                for (int i = 0; i < pixelSize; i++)
                {
                    sdl.RenderDrawLine(renderer, pixelX, pixelY + i, pixelX + pixelSize, pixelY + i);
                }
            }
        }
    }
    // end AI-generated
}