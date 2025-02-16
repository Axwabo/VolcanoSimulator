using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class AshCloudRenderer : EruptedMaterialRenderer
{

    public AshCloudRenderer(AshCloud material) : base(material)
    {
    }

    public override void Draw(in ViewportRect viewport) => throw new NotImplementedException();

    public override void Clear(in ViewportRect viewport) => throw new NotImplementedException();

}
