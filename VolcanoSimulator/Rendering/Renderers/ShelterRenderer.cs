using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class ShelterRenderer : SymbolBasedRenderer
{

    public ShelterRenderer(EvacuationShelter landmark) : base(landmark, 'E')
    {
    }

}
