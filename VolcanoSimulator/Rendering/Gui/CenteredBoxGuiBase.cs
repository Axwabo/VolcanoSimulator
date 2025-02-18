namespace VolcanoSimulator.Rendering.Gui;

public abstract class CenteredBoxGuiBase : GuiBase
{

    private const string Top = "╔═════════════════════════╗";
    private const string Side = "║";
    private const string Bottom = "╚═════════════════════════╝";

    protected const int RowWidth = 25;

    protected abstract string[] Rows { get; }

    protected virtual int DrawRow(int i, string option)
    {
        Console.CursorLeft++;
        Console.Write(option);
        return option.Length + 1;
    }

    public override void Draw()
    {
        var row = Console.WindowHeight / 2 - Rows.Length / 2 - 1;
        var center = Console.WindowWidth / 2;
        Console.SetCursorPosition(center - Top.Length / 2, row);
        Console.Write(Top);
        for (var i = 0; i < Rows.Length; i++)
        {
            var option = Rows[i];
            Console.SetCursorPosition(center - RowWidth / 2 - 1, ++row);
            Console.Write(Side);
            var width = DrawRow(i, option);
            Erase.TextFromCursor(RowWidth - width);
            Console.Write(Side);
        }

        Console.SetCursorPosition(center - Bottom.Length / 2, ++row);
        Console.Write(Bottom);
    }

}
