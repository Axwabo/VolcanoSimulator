using System.Buffers;

namespace VolcanoSimulator.Rendering.Gui;

public class InputField
{

    private readonly int _row;
    private readonly char[] _buffer;

    public int Length { get; private set; }

    public Span<char> Text => _buffer.AsSpan()[..Length];

    public InputField(int row, int maxLength)
    {
        _row = row;
        _buffer = ArrayPool<char>.Shared.Rent(maxLength);
    }

    public void Draw()
    {
        Render.TextRight(1, Text);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
    }

    public GuiInputResult ProcessInput(in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
            return GuiInputResult.Exit;
        if (key.Key != ConsoleKey.Backspace)
        {
            if (Length == _buffer.Length || !IsAllowed(key.KeyChar))
                return GuiInputResult.None;
            Clear();
            Append(key.KeyChar);
            Write();
            return GuiInputResult.None;
        }

        if (Length == 0)
            return GuiInputResult.None;
        Console.SetCursorPosition(Console.WindowWidth - Length--, 1);
        Console.Write(' ');
        Write();
        return GuiInputResult.None;
    }

    protected void Append(char character) => _buffer[Length++] = character;

    protected virtual bool IsAllowed(char character) => true;

    private void Clear() => Erase.TextRight(_row, Length);

    private void Write()
    {
        Render.TextRight(1, Text);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
    }

    ~InputField() => ArrayPool<char>.Shared.Return(_buffer);

}
