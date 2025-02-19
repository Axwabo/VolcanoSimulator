using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering.Extensions;

public static class SurvivorGroupRenderingExtensions
{

    public static SurvivorGroupRenderer GetRenderer(this RendererTable renderers, SurvivorGroup group)
    {
        if (renderers.TryGetValue(group, out var baseRenderer) && baseRenderer is SurvivorGroupRenderer renderer)
            return renderer;
        renderer = new SurvivorGroupRenderer(group);
        renderers.Add(group, renderer);
        return renderer;
    }

    public static void ClearAll(this IEnumerable<SurvivorGroup> groups, RendererTable table, in ViewportRect viewport)
    {
        foreach (var group in groups)
            table.GetRenderer(group).Clear(viewport);
    }

    public static void DrawAll(this IEnumerable<SurvivorGroup> groups, RendererTable table, in ViewportRect viewport)
    {
        foreach (var group in groups)
            table.GetRenderer(group).Clear(viewport);
    }

}
