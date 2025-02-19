using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Rendering.Renderers.Landmarks;

namespace VolcanoSimulator.Rendering.Extensions;

public static class EruptedMaterialRenderingExtensions
{

    public static IEnumerable<EruptedMaterialRenderer> GetRenderers(this IEnumerable<EruptedMaterialBase> materials, RendererTable table)
        => materials.Select(table.GetRenderer);

    public static EruptedMaterialRenderer GetRenderer(this RendererTable renderers, EruptedMaterialBase material)
    {
        if (renderers.TryGetValue(material, out var baseRenderer) && baseRenderer is EruptedMaterialRenderer renderer)
            return renderer;
        renderer = material switch
        {
            AshCloud ashCloud => new AshCloudRenderer(ashCloud),
            Lava lava => new LavaRenderer(lava),
            _ => throw new NotSupportedException($"Cannot create a renderer for {material.GetType().FullName}")
        };
        renderers.Add(material, renderer);
        return renderer;
    }

    public static void ClearAll(this IEnumerable<EruptedMaterialBase> materials, RendererTable table, in ViewportRect viewport, MaterialLayer layers)
    {
        foreach (var renderer in materials.GetRenderers(table).Filter(layers).OrderByLayerDescending())
            renderer.Clear(viewport);
    }

    public static void DrawAll(this IEnumerable<EruptedMaterialBase> materials, RendererTable table, in ViewportRect viewport, MaterialLayer layers)
    {
        foreach (var renderer in materials.GetRenderers(table).Filter(layers).OrderByLayer())
            renderer.Draw(viewport);
    }

}
