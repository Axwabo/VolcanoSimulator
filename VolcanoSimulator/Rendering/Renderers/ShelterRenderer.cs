using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class ShelterRenderer : LandmarkRenderer
{

    public ShelterRenderer(EvacuationShelter landmark) : base(landmark)
    {
    }

    public override void Draw(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write('E');
    }

    public override void Clear(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write(' ');
    }

}
