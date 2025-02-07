using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering;

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
            case ConsoleKey.UpArrow or ConsoleKey.W:
                coordinates = Up;
                return true;
            case ConsoleKey.DownArrow or ConsoleKey.S:
                coordinates = Down;
                return true;
            case ConsoleKey.LeftArrow or ConsoleKey.A:
                coordinates = Left;
                return true;
            case ConsoleKey.RightArrow or ConsoleKey.D:
                coordinates = Right;
                return true;
            default:
                coordinates = default;
                return false;
        }
    }

}
