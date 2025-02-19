namespace VolcanoSimulator.Rendering.Gui;

public interface IActionModeModifier
{

    ActionMode Mode { get; }

    string? PrimaryAction => null; // TODO: show in status bar

}
