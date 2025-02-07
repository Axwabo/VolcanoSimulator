using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering;

public readonly record struct ViewportRect(int Width, int Height, int X, int Y)
{

    public Coordinates Size => new(Height, Width);

    public bool Contains(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height;

    public bool TryTransform(in Coordinates world, out Coordinates screen)
    {
        var x = world.Longitude - X;
        var y = world.Latitude - Y;
        if (!Contains(x, y))
        {
            screen = default;
            return false;
        }

        screen = new Coordinates(y, x);
        return true;
    }

}
