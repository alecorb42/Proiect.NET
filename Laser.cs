using Silk.NET.SDL;

namespace TheAdventure;

public class Laser : GameObject
{
    private readonly double _speed = 500; 

    public Laser(double x, double y) : base(x, y, 4, 15)
    {
    }

    public override void Update(double deltaTime)
    {
        Y -= _speed * deltaTime;
        if (Y < -Height)
        {
            IsActive = false;
        }
    }

    public override unsafe void Render(Sdl sdl, Renderer* renderer)
    {
        sdl.SetRenderDrawColor(renderer, 255, 200, 0, 255);
        
        int currentX = (int)X;
        int currentY = (int)Y;

        for (int i = 0; i < Width; i++)
        {
            sdl.RenderDrawLine(renderer, currentX + i, currentY, currentX + i, currentY + Height);
        }
    }
}