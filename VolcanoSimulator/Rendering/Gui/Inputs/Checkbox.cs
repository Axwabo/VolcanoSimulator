namespace VolcanoSimulator.Rendering.Gui.Inputs;

public sealed class Checkbox : IInputField
{

    private const string CheckedText = "[X]";
    private const string UncheckedText = "[ ]";

    public int Row { get; }
    public bool Checked { get; private set; }
    private string Text => Checked ? CheckedText : UncheckedText;

    public Checkbox(int row, bool @checked)
    {
        Row = row;
        Checked = @checked;
    }

    public void Draw(bool active)
    {
        Render.TextRight(Row, Text);
        if (active)
            Console.SetCursorPosition(Console.WindowWidth - 2, Row);
    }

    public GuiInputResult ProcessInput(in ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Spacebar:
                Checked = !Checked;
                Draw(true);
                return GuiInputResult.None;
            case ConsoleKey.Enter or ConsoleKey.Escape:
                return GuiInputResult.Exit;
            default:
                return GuiInputResult.None;
        }
    }

}
