namespace VolcanoSimulator.Rendering.Gui;

public sealed class ControlsGui : CenteredBoxGuiBase
{

    private static readonly string[] List =
    [
        "WASD/↑←↓→: Move",
        "Shift: Fast Move",
        "E: Place shelter",
        "C: place city",
        "V: Place volcano"
    ];

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
        => key.Key is ConsoleKey.Escape or ConsoleKey.Enter ? GuiInputResult.Exit : GuiInputResult.None;

    protected override string[] Rows => List;

}
