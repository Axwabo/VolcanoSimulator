namespace VolcanoSimulator.Rendering;

public static class ModeToString
{

    public static string ToStringFast(this ActionMode mode) => mode switch
    {
        ActionMode.Normal => "NORMAL",
        ActionMode.Insert => "INSERT",
        ActionMode.Step => "STEP",
        ActionMode.Rescue => "RESCUE",
        _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Unknown action mode")
    };

}
