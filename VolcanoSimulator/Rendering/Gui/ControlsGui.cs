﻿namespace VolcanoSimulator.Rendering.Gui;

public sealed class ControlsGui : CenteredBoxGuiBase
{

    private static readonly string[] List =
    [
        "Enter: Interact",
        "W/A/S/D: Move",
        "Shift: Fast Move",
        "-- NORMAL --",
        "E: Place shelter",
        "C: place city",
        "V: Place volcano",
        "-- STEP --",
        "I: Increase time",
        "Space: Simulate step"
    ];

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
        => key.Key is ConsoleKey.Escape or ConsoleKey.Enter ? GuiInputResult.Exit : GuiInputResult.None;

    protected override string[] Rows => List;

}
