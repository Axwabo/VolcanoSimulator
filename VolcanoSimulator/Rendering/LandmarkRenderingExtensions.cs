using System.Diagnostics.CodeAnalysis;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering;

public static class LandmarkRenderingExtensions
{

    public static LandmarkRenderer GetRenderer(this RendererTable renderers, LandmarkBase landmark)
    {
        if (renderers.TryGetValue(landmark, out var baseRenderer) && baseRenderer is LandmarkRenderer renderer)
            return renderer;
        renderer = landmark switch
        {
            City city => new CityRenderer(city),
            Volcano volcano => new VolcanoRenderer(volcano),
            EvacuationShelter evacuationShelter => new ShelterRenderer(evacuationShelter),
            _ => throw new NotSupportedException($"Cannot create a renderer for {landmark.GetType().FullName}")
        };
        renderers.Add(landmark, renderer);
        return renderer;
    }

    public static void ClearAll(this IEnumerable<LandmarkBase> landmarks, RendererTable table, in ViewportRect viewport)
    {
        foreach (var landmark in landmarks)
            table.GetRenderer(landmark).Clear(viewport);
    }

    public static bool DrawAllAndTryGetSelected(
        this IEnumerable<LandmarkBase> landmarks,
        RendererTable table,
        in ViewportRect viewport,
        Coordinates center,
        [NotNullWhen(true)] out LandmarkBase? selected
    )
    {
        selected = null;
        foreach (var landmark in landmarks)
        {
            table.GetRenderer(landmark).Draw(viewport);
            if (viewport.TryTransform(landmark.Location, out var screen) && screen == center)
                selected = landmark;
        }

        return selected is not null;
    }

}
