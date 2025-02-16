using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering;

public static class EruptedMaterialRenderingExtensions
{

    public static EruptedMaterialRenderer GetRenderer(this RendererTable renderers, EruptedMaterialBase material)
    {
        if (renderers.TryGetValue(material, out var baseRenderer) && baseRenderer is EruptedMaterialRenderer renderer)
            return renderer;
        renderer = material switch
        {
            AshCloud ashCloud => new AshCloudRenderer(ashCloud),
            _ => throw new NotSupportedException($"Cannot create a renderer for {material.GetType().FullName}")
        };
        renderers.Add(material, renderer);
        return renderer;
    }

    public static void ClearAll(this IEnumerable<EruptedMaterialBase> landmarks, RendererTable table, in ViewportRect viewport)
    {
        foreach (var landmark in landmarks)
            GetRenderer(table, landmark).Clear(viewport);
    }

    public static void DrawAll(this IEnumerable<EruptedMaterialBase> landmarks, RendererTable table, in ViewportRect viewport)
    {
        foreach (var landmark in landmarks)
            GetRenderer(table, landmark).Draw(viewport);
    }

}
