namespace VolcanoSimulator.Rendering.Renderers.Landmarks;

public abstract class EruptedMaterialRenderer : PositionedRenderer
{

    public abstract MaterialLayer Layer { get; }

    protected EruptedMaterialRenderer(EruptedMaterialBase material) : base(material)
    {
    }

}
