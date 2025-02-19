using VolcanoSimulator.Rendering.Renderers.Landmarks;

namespace VolcanoSimulator.Rendering.Renderers;

public static class LayerExtensions
{

    public static IEnumerable<EruptedMaterialRenderer> Filter(this IEnumerable<EruptedMaterialRenderer> renderers, MaterialLayer layers)
        => renderers.Where(e => layers.HasFlagFast(e.Layer));

    public static IEnumerable<EruptedMaterialRenderer> OrderByLayer(this IEnumerable<EruptedMaterialRenderer> renderers)
        => renderers.OrderBy(e => e.Layer);

    public static IEnumerable<EruptedMaterialRenderer> OrderByLayerDescending(this IEnumerable<EruptedMaterialRenderer> renderers)
        => renderers.OrderByDescending(e => e.Layer);

    public static bool HasFlagFast(this MaterialLayer layers, MaterialLayer flag) => (layers & flag) != 0;

}
