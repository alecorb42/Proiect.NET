using Silk.NET.SDL;

namespace TheAdventure;

public abstract class GameObject
{
    public double X { get; set; }
    public double Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsActive { get; set; } = true;

    protected GameObject(double x, double y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
    public bool CheckCollision(GameObject other)
    {
        return IsActive && other.IsActive &&
               X < other.X + other.Width &&
               X + Width > other.X &&
               Y < other.Y + other.Height &&
               Y + Height > other.Y;
    }
    public abstract void Update(double deltaTime);

    public abstract unsafe void Render(Sdl sdl, Renderer* renderer);
}