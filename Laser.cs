using Silk.NET.SDL;

namespace TheAdventure;

public class Laser : GameObject
{
    private readonly double _speed = 500; // Viteza cu care zboară în sus

    public Laser(double x, double y) : base(x, y, 4, 15)
    {
    }

    public override void Update(double deltaTime)
    {
        // Se mișcă în sus, deci scadem din Y
        Y -= _speed * deltaTime;

        // Dacă iese de pe ecran, îl dezactivăm ca să îl ștergem din memorie
        if (Y < -Height)
        {
            IsActive = false;
        }
    }

    public override unsafe void Render(Sdl sdl, Renderer* renderer)
    {
        // Îl desenăm ca o linie verticală strălucitoare (galbenă/portocalie)
        sdl.SetRenderDrawColor(renderer, 255, 200, 0, 255);
        
        int currentX = (int)X;
        int currentY = (int)Y;

        for (int i = 0; i < Width; i++)
        {
            sdl.RenderDrawLine(renderer, currentX + i, currentY, currentX + i, currentY + Height);
        }
    }
}