using System.Diagnostics.CodeAnalysis;
using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering;

public static class LandmarkRenderingExtensions
{

    public const string Name = "Name: ";

    public const string People = "People: ";

    public static void ClearAll(this IEnumerable<LandmarkBase> landmarks, in ViewportRect viewport)
    {
        foreach (var landmark in landmarks)
            switch (landmark)
            {
                case City city:
                    CityRenderer.Clear(city, viewport);
                    break;
            }
    }

    public static bool DrawAllAndTryGetSelected(
        this IEnumerable<LandmarkBase> landmarks,
        in ViewportRect viewport,
        Coordinates center,
        [NotNullWhen(true)] out LandmarkBase? selected
    )
    {
        selected = null;
        foreach (var landmark in landmarks)
        {
            switch (landmark)
            {
                case City city:
                    CityRenderer.Draw(city, viewport);
                    break;
            }

            if (viewport.TryTransform(landmark.Location, out var screen) && screen == center)
                selected = landmark;
        }

        return selected is not null;
    }

    public static void ClearInfo(this LandmarkBase landmark)
    {
        if (landmark is City city)
            Erase.TextRight(0, Name.Length + city.Name.Length);
        if (landmark is IEvacuationLocation evacuationLocation)
            Erase.TextRight(1, People.Length + IntLength(evacuationLocation.AccommodatedPeople));
    }

    public static void DrawInfo(this LandmarkBase landmark)
    {
        if (landmark is City city)
            Render.TextRight(0, Name, city.Name);
        if (landmark is IEvacuationLocation evacuationLocation)
            Render.TextRight(1, People, evacuationLocation.AccommodatedPeople.ToString());
    }

    private static int IntLength(int value) => value == 0 ? 1 : (int) Math.Floor(Math.Log10(value)) + 1;

}
