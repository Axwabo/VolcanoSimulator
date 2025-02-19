namespace VolcanoSimulator.Rendering.Gui;

public static class LandmarkActionExtensions
{

    public static string? GetNormalAction(this LandmarkBase? landmark) => landmark switch
    {
        City => "[ENTER] Add citizens",
        Volcano => "[ENTER] Erupt",
        _ => null
    };

}
