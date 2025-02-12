using System.Diagnostics.CodeAnalysis;
using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering;

public static class LandmarkRenderingExtensions
{

    public const string Name = "Name: ";

    public const string People = "People: ";

    public static LandmarkRenderer GetRenderer(this RendererTable renderers, LandmarkBase landmark)
    {
        if (renderers.TryGetValue(landmark, out var renderer) && renderer != null)
            return renderer;
        renderer = landmark switch
        {
            City city => new CityRenderer(city),
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

    public static void ClearInfo(this LandmarkBase landmark)
    {
        if (landmark is NamedLandmark named)
            Erase.TextRight(0, Name.Length + named.Name.Length);
        else
            Erase.TextRight(0, landmark.GetType().Name.Length);
        if (landmark is IEvacuationLocation evacuationLocation)
            Erase.TextRight(1, People.Length + IntLength(evacuationLocation.AccommodatedPeople));
    }

    public static void DrawInfo(this LandmarkBase landmark)
    {
        if (landmark is NamedLandmark named)
            Render.TextRight(0, Name, named.Name);
        else
            Render.TextRight(0, landmark.GetType().Name);
        if (landmark is IEvacuationLocation evacuationLocation)
            Render.TextRight(1, People, evacuationLocation.AccommodatedPeople.ToString());
    }

    private static int IntLength(int value) => value == 0 ? 1 : (int) Math.Floor(Math.Log10(value)) + 1;

}
