﻿namespace VolcanoSimulator.Rendering.Extensions;

public static class InputExtensions
{

    private static readonly Coordinates Up = new(-1, 0);
    private static readonly Coordinates Down = new(1, 0);
    private static readonly Coordinates Left = new(0, -1);
    private static readonly Coordinates Right = new(0, 1);

    public static bool TryGetMovementDelta(this ConsoleKey key, out Coordinates coordinates)
    {
        switch (key)
        {
            case ConsoleKey.W:
                coordinates = Up;
                return true;
            case ConsoleKey.S:
                coordinates = Down;
                return true;
            case ConsoleKey.A:
                coordinates = Left;
                return true;
            case ConsoleKey.D:
                coordinates = Right;
                return true;
            default:
                coordinates = default;
                return false;
        }
    }

    public static bool IsControl(this ConsoleKeyInfo key) => key.Modifiers.HasFlagFast(ConsoleModifiers.Control);

    public static bool IsShift(this ConsoleKeyInfo key) => key.Modifiers.HasFlagFast(ConsoleModifiers.Shift);

    public static bool HasFlagFast(this ConsoleModifiers modifiers, ConsoleModifiers match) => (modifiers & match) != 0;

}
