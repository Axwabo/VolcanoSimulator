using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public struct ShelterRenderer
{

    public static void Draw(EvacuationShelter target, in ViewportRect viewport)
    {
        if (SetPosition(viewport, target))
            Console.Write('E');
    }

    public static void Clear(EvacuationShelter target, in ViewportRect viewport)
    {
        if (SetPosition(viewport, target))
            Console.Write(' ');
    }

    private static bool SetPosition(in ViewportRect viewport, EvacuationShelter city)
    {
        if (!viewport.TryTransform(city.Location, out var screen))
            return false;
        Render.Cursor = screen;
        return true;
    }

}
