namespace VolcanoSimulator.Rendering.Gui;

public sealed class MenuGui : CenteredBoxGuiBase
{

    private const string Selected = " > ";

    protected override string[] Rows { get; } =
    [
        "Controls",
        "Exit"
    ];

    private int _optionIndex;

    protected override int DrawRow(int i, string option)
    {
        if (i == _optionIndex)
            Console.Write(Selected);
        else
            Erase.TextFromCursor(Selected.Length);
        Console.Write(option);
        return Selected.Length + option.Length;
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
        => key.Key switch
        {
            ConsoleKey.Enter => Select(renderer),
            ConsoleKey.Escape => GuiInputResult.Exit,
            ConsoleKey.UpArrow => CycleSelection(-1),
            ConsoleKey.DownArrow => CycleSelection(1),
            _ => GuiInputResult.None
        };

    private GuiInputResult Select(SimulatorRenderer renderer)
    {
        switch (_optionIndex)
        {
            case 0:
                renderer.ShowGui(new ControlsGui {Parent = this});
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
        var firstRow = Console.WindowHeight / 2 - Rows.Length / 2 + 1;
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
        if (_optionIndex >= Rows.Length)
            _optionIndex = 0;
        else if (_optionIndex < 0)
            _optionIndex = Rows.Length - 1;
    }

}
