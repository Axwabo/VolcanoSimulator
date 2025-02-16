using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public abstract class EruptedMaterialRenderer : PositionedRenderer
{

    protected EruptedMaterialRenderer(EruptedMaterialBase material) : base(material)
    {
    }

}
