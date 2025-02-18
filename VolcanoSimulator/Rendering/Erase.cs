namespace VolcanoSimulator.Rendering;

public static class Erase
{

    private static readonly string EmptyBuffer = new(' ', 64);

    public static void TextRight(int row, int amount) => Render.TextRight(row, EmptyBuffer.AsSpan()[..amount]);

    public static void SelectionIndicator(Coordinates coordinates)
    {
        Render.Cursor = coordinates;
        Console.CursorLeft--;
        Console.Write(' ');
        Console.CursorLeft++;
        Console.Write(' ');
    }

    public static void TextFromCursor(int amount)
    {
        if (amount > 0)
            Console.Out.Write(EmptyBuffer.AsSpan()[..amount]);
    }

    public static void SimulatorLocation(int length, int previousEarthquakeLength)
    {
        Console.SetCursorPosition(0, 1);
        TextFromCursor(length);
        if (previousEarthquakeLength == 0)
            return;
        Console.SetCursorPosition(0, 2);
        TextFromCursor(previousEarthquakeLength);
    }

}
