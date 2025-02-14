namespace VolcanoSimulator.Rendering.Gui;

public sealed class MenuGui : GuiBase
{

    private const int RowWidth = 13;
    private const string Top = "╔═════════════╗";
    private const string Side = "║";
    private const string Bottom = "╚═════════════╝";
    private const string Selected = " > ";

    private static readonly string[] Options =
    [
        "Controls",
        "Exit"
    ];

    private int _optionIndex;

    public override void Draw()
    {
        var row = Console.WindowHeight / 2 - Options.Length / 2;
        var center = Console.WindowWidth / 2;
        Console.SetCursorPosition(center - Top.Length / 2, row);
        Console.Write(Top);
        for (var i = 0; i < Options.Length; i++)
        {
            var option = Options[i];
            Console.SetCursorPosition(center - RowWidth / 2 - 1, ++row);
            Console.Write(Side);
            if (i == _optionIndex)
                Console.Write(Selected);
            else
                Erase.TextFromCursor(Selected.Length);
            Console.Write(option);
            Erase.TextFromCursor(RowWidth - option.Length - Selected.Length);
            Console.Write(Side);
        }

        Console.SetCursorPosition(center - Bottom.Length / 2, ++row);
        Console.Write(Bottom);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
        => key.Key switch
        {
            ConsoleKey.Enter => Select(),
            ConsoleKey.Escape => GuiInputResult.Exit,
            ConsoleKey.UpArrow => CycleSelection(-1),
            ConsoleKey.DownArrow => CycleSelection(1),
            _ => GuiInputResult.None
        };

    private GuiInputResult Select()
    {
        switch (_optionIndex)
        {
            case 0:
                // show controls gui
                return GuiInputResult.None;
            case 1:
                return GuiInputResult.QuitGame;
            default:
                return GuiInputResult.None;
        }
    }

    private GuiInputResult CycleSelection(int deltaY)
    {
        var startLeft = Console.WindowWidth / 2 - RowWidth / 2;
        var firstRow = Console.WindowHeight / 2 - Options.Length / 2 + 1;
        Console.SetCursorPosition(startLeft, _optionIndex + firstRow);
        Erase.TextFromCursor(Selected.Length);
        CycleIndexAndWrap(deltaY);
        Console.SetCursorPosition(startLeft, _optionIndex + firstRow);
        Console.Write(Selected);
        return GuiInputResult.None;
    }

    private void CycleIndexAndWrap(int deltaY)
    {
        _optionIndex += deltaY;
        if (_optionIndex >= Options.Length)
            _optionIndex = 0;
        else if (_optionIndex < 0)
            _optionIndex = Options.Length - 1;
    }

}
