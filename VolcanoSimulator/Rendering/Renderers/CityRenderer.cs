using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class CityRenderer : LandmarkRenderer
{

    public CityRenderer(City landmark) : base(landmark)
    {
    }

    public override void Draw(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write('C');
    }

    public override void Clear(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write(' ');
    }

}
