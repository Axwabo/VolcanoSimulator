using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering;

public static class LandmarkInfoRenderingExtensions
{

    private const string Name = "Name: ";
    private const string People = "People: ";
    private const string Capacity = "Capacity: ";

    public static void ClearInfo(this LandmarkBase landmark)
    {
        var row = 0;
        var typeLength = landmark.GetType().Name.Length;
        Erase.TextRight(row++, typeLength);
        if (landmark is NamedLandmark named)
            Erase.TextRight(row++, Name.Length + named.Name.Length);
        if (landmark is not IEvacuationLocation evacuationLocation)
            return;
        Erase.TextRight(row++, People.Length + IntLength(evacuationLocation.AccommodatedPeople));
        if (evacuationLocation.ShelterCapacity != int.MaxValue)
            Erase.TextRight(row, Capacity.Length + IntLength(evacuationLocation.ShelterCapacity));
    }

    public static void DrawInfo(this LandmarkBase landmark)
    {
        var row = 0;
        var type = landmark.GetType().Name;
        Render.TextRight(row++, type);
        if (landmark is NamedLandmark named)
            Render.TextRight(row++, Name, named.Name);
        if (landmark is not IEvacuationLocation evacuationLocation)
            return;
        Render.TextRight(row++, People, evacuationLocation.AccommodatedPeople.ToString());
        if (evacuationLocation.ShelterCapacity != int.MaxValue)
            Render.TextRight(row, Capacity, evacuationLocation.ShelterCapacity.ToString());
    }

    public static bool TryClearSelectedLandmark(this SimulatorRenderer renderer)
    {
        if (renderer.SelectedLandmark == null)
            return false;
        var center = renderer.Viewport.Size / 2;
        Erase.SelectionIndicator(center);
        renderer.SelectedLandmark.ClearInfo();
        Render.Cursor = center;
        Console.Write('+');
        return true;
    }

    private static int IntLength(int value) => value == 0 ? 1 : (int) Math.Floor(Math.Log10(value)) + 1;

}
