using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public readonly record struct CityRenderer : IRenderer
{

    private readonly City _city;

    public CityRenderer(City city) => _city = city;

    public void Draw(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write('C');
    }

    public void Clear(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write(' ');
    }

    private bool SetPosition(in ViewportRect viewport)
    {
        if (!viewport.TryTransform(_city.Location, out var screen))
            return false;
        Console.SetCursorPosition(screen.Longitude, screen.Latitude);
        return true;
    }

}
