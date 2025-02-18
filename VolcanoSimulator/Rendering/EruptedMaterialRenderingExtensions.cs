using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering;

public static class EruptedMaterialRenderingExtensions
{

    public static IEnumerable<EruptedMaterialRenderer> GetRenderers(this IEnumerable<EruptedMaterialBase> materials, RendererTable table)
        => materials.Select(table.GetRenderer);

    public static IEnumerable<EruptedMaterialRenderer> FilterByLayer(this IEnumerable<EruptedMaterialRenderer> renderers, MaterialLayer layers)
        => renderers.Where(e => (layers & e.Layer) != 0);

    public static IEnumerable<EruptedMaterialRenderer> OrderByLayer(this IEnumerable<EruptedMaterialRenderer> renderers)
        => renderers.OrderBy(e => e.Layer);

    public static IEnumerable<EruptedMaterialRenderer> OrderByLayerDescending(this IEnumerable<EruptedMaterialRenderer> renderers)
        => renderers.OrderByDescending(e => e.Layer);

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

    public static void ClearAll(this IEnumerable<EruptedMaterialBase> materials, RendererTable table, in ViewportRect viewport)
    {
        foreach (var renderer in materials.GetRenderers(table).OrderByLayerDescending())
            renderer.Clear(viewport);
    }

    public static void DrawAll(this IEnumerable<EruptedMaterialBase> materials, RendererTable table, in ViewportRect viewport)
    {
        foreach (var renderer in materials.GetRenderers(table).OrderByLayer())
            renderer.Draw(viewport);
    }

}
