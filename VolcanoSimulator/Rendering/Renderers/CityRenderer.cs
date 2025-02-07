using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public struct CityRenderer
{

    public static void Draw(City target, in ViewportRect viewport)
    {
        if (SetPosition(viewport, target))
            Console.Write('C');
    }

    public static void Clear(City target, in ViewportRect viewport)
    {
        if (SetPosition(viewport, target))
            Console.Write(' ');
    }

    private static bool SetPosition(in ViewportRect viewport, City city)
    {
        if (!viewport.TryTransform(city.Location, out var screen))
            return false;
        Render.Cursor = screen;
        return true;
    }

}
