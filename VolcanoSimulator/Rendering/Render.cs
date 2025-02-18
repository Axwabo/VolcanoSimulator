using UnitsNet.Units;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering;

public static class Render
{

    public static Coordinates Cursor
    {
        set => Console.SetCursorPosition(value.Longitude, value.Latitude);
    }

    public static void TextRight(int row, in ReadOnlySpan<char> text)
    {
        if (text.Length == 0)
            return;
        Console.SetCursorPosition(Console.WindowWidth - text.Length, row);
        Console.Out.Write(text);
    }

    public static void TextRight(int row, in ReadOnlySpan<char> text1, in ReadOnlySpan<char> text2)
    {
        var length = text1.Length + text2.Length;
        if (length == 0)
            return;
        Console.SetCursorPosition(Console.WindowWidth - length, row);
        Console.Out.Write(text1);
        Console.Out.Write(text2);
    }

    public static void SelectionIndicator(Coordinates coordinates)
    {
        Cursor = coordinates;
        Console.CursorLeft--;
        Console.Write('>');
        Console.CursorLeft++;
        Console.Write('<');
    }

    public static int SimulatorInfo(in ViewportRect viewport, Coordinates location)
    {
        var (y, x) = location + viewport.Size / 2;
        Console.SetCursorPosition(0, 0);
        Console.Write("Volcano Simulator");
        Console.SetCursorPosition(0, 1);
        Console.Write((x * PositionedRenderer.PixelSize).As(LengthUnit.Meter));
        Console.Write("m; ");
        Console.Write((y * PositionedRenderer.PixelSize).As(LengthUnit.Meter));
        Console.Write('m');
        return Console.CursorLeft;
    }

}
