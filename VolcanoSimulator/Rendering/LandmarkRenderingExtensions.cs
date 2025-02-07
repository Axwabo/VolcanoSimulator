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
                    new CityRenderer(city).Clear(viewport);
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
                    new CityRenderer(city).Draw(viewport);
                    break;
            }

            if (viewport.TryTransform(landmark.Location, out var screen) && screen != center)
                selected = landmark;
        }

        return selected is not null;
    }

    public static void ClearInfo(this LandmarkBase landmark, in ViewportRect viewport)
    {
        if (landmark is City city)
        {
            var length = Name.Length + city.Name.Length;
            Console.SetCursorPosition(viewport.Width - length, 0);
            for (var i = 0; i < length; i++)
                Console.Write(' ');
        }

        if (landmark is IEvacuationLocation evacuationLocation)
        {
            var length = People.Length + IntLength(evacuationLocation.AccommodatedPeople);
            Console.SetCursorPosition(viewport.Width - length, 1);
            for (var i = 0; i < length; i++)
                Console.Write(' ');
        }
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
