using Silk.NET.SDL;

namespace TheAdventure;

public class PlayerShip : GameObject
{
    private readonly double _speed = 300; 

    public PlayerShip(double x, double y) : base(x, y, 40, 40){}

    public void MoveLeft(double deltaTime)
    {
        X -= _speed * deltaTime;
        if (X < 0) X = 0; 
    }

    public void MoveRight(double deltaTime, int screenWidth)
    {
        X += _speed * deltaTime;
        if (X > screenWidth - Width) X = screenWidth - Width; 
    }

    public override void Update(double deltaTime){}

    public override unsafe void Render(Sdl sdl, Renderer* renderer)
    {
        // Matrice 15x15 pentru o navă spațială detaliată
        // 0 = Transparent, 1 = Negru (Contur), 2 = Roșu Închis, 3 = Roșu Deschis, 4 = Gri Închis (Cockpit), 5 = Portocaliu (Foc/Propulsie)
        int[,] sprite = new int[,] {
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 3, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 3, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 2, 4, 2, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 2, 4, 2, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0 },
            { 0, 1, 3, 3, 2, 2, 2, 2, 2, 2, 2, 3, 3, 1, 0 },
            { 1, 3, 3, 3, 3, 1, 1, 2, 1, 1, 3, 3, 3, 3, 1 },
            { 1, 2, 2, 1, 1, 0, 1, 2, 1, 0, 1, 1, 2, 2, 1 },
            { 0, 1, 1, 0, 0, 0, 1, 2, 1, 0, 0, 0, 1, 1, 0 },
            { 0, 0, 0, 0, 0, 1, 2, 2, 2, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 5, 1, 5, 1, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 1, 5, 1, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }
        };

        // Reducem dimensiunea pixelului la 3 ca nava să nu fie uriașă pe ecran, dar să fie fină
        int pixelSize = 6; 

        for (int row = 0; row < sprite.GetLength(0); row++)
        {
            for (int col = 0; col < sprite.GetLength(1); col++)
            {
                int colorCode = sprite[row, col];

                if (colorCode == 0) continue;

                switch (colorCode)
                {
                    case 1: sdl.SetRenderDrawColor(renderer, 20, 20, 20, 255); break;     // Contur Negru închis
                    case 2: sdl.SetRenderDrawColor(renderer, 160, 20, 20, 255); break;   // Roșu Închis (Corp/Umbră)
                    case 3: sdl.SetRenderDrawColor(renderer, 230, 40, 40, 255); break;   // Roșu Aprins (Aripă/Lumină)
                    case 4: sdl.SetRenderDrawColor(renderer, 60, 60, 80, 255); break;     // Gri Albăstrui (Parbriz/Cockpit)
                    case 5: sdl.SetRenderDrawColor(renderer, 255, 140, 0, 255); break;   // Portocaliu Foc (Motoare)
                }

                int pixelX = (int)X + (col * pixelSize);
                int pixelY = (int)Y + (row * pixelSize);

                // Desenăm pixelul ca un pătrățel plin
                for (int i = 0; i < pixelSize; i++)
                {
                    sdl.RenderDrawLine(renderer, pixelX, pixelY + i, pixelX + pixelSize, pixelY + i);
                }
            }
        }
    }
}