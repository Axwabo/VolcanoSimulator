namespace VolcanoSimulator.Rendering.Gui;

public sealed class MenuGui : GuiBase
{

    private const string Top = "╔═══════════╗";
    private const string Side = "║";
    private const string Exit = $"{Side} > Exit    {Side}";
    private const string Bottom = "╚═══════════╝";

    public override void Draw()
    {
        var centerRow = Console.WindowHeight / 2;
        var centerCol = Console.WindowWidth / 2;
        Console.SetCursorPosition(centerCol - Top.Length / 2, centerRow - 1);
        Console.Write(Top);
        Console.SetCursorPosition(centerCol - Top.Length / 2, centerRow);
        Console.Write(Exit);
        Console.SetCursorPosition(centerCol - Top.Length / 2, centerRow + 1);
        Console.Write(Bottom);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
        => key.Key switch
        {
            ConsoleKey.Enter => GuiInputResult.QuitGame,
            ConsoleKey.Escape or ConsoleKey.X => GuiInputResult.Exit,
            _ => GuiInputResult.None
        };

}
