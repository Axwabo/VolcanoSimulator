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
        var x = _city.Location.Longitude - viewport.X;
        var y = _city.Location.Latitude - viewport.Y;
        if (x < 0 || x >= viewport.Width || y < 0 || y >= viewport.Height)
            return false;
        Console.SetCursorPosition(x, y);
        return true;
    }

}
