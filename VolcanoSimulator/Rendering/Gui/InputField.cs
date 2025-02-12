using System.Buffers;

namespace VolcanoSimulator.Rendering.Gui;

public class InputField
{

    private readonly int _row;
    private readonly char[] _buffer;

    public int MaxLength { get; }

    public int Length { get; private set; }

    public Span<char> Text => _buffer.AsSpan()[..Length];

    public InputField(int row, int maxLength)
    {
        _row = row;
        _buffer = ArrayPool<char>.Shared.Rent(maxLength);
        MaxLength = maxLength;
    }

    public void Draw(bool active)
    {
        Render.TextRight(_row, Text);
        if (active)
            Console.SetCursorPosition(Console.WindowWidth - 1, _row);
    }

    public GuiInputResult ProcessInput(in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
            return GuiInputResult.Exit;
        if (key.Key != ConsoleKey.Backspace)
        {
            if (Length == MaxLength || !IsAllowed(key.KeyChar))
                return GuiInputResult.None;
            Clear();
            Append(key.KeyChar);
            Write();
            return GuiInputResult.None;
        }

        if (Length == 0)
            return GuiInputResult.None;
        Console.SetCursorPosition(Console.WindowWidth - Length--, _row);
        Console.Write(' ');
        Write();
        return GuiInputResult.None;
    }

    protected void Append(char character) => _buffer[Length++] = character;

    protected virtual bool IsAllowed(char character) => true;

    private void Clear() => Erase.TextRight(_row, Length);

    private void Write()
    {
        Render.TextRight(_row, Text);
        Console.SetCursorPosition(Console.WindowWidth - 1, _row);
    }

    ~InputField() => ArrayPool<char>.Shared.Return(_buffer);

}
