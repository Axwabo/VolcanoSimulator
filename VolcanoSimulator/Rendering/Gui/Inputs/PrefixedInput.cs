namespace VolcanoSimulator.Rendering.Gui.Inputs;

public sealed class PrefixedInput<T> : IInputField where T : InputField
{

    private readonly string _prefix;

    public T Input { get; }

    private int _previousLength;

    public PrefixedInput(T input, string prefix)
    {
        _prefix = prefix;
        Input = input;
    }

    public void Draw(bool active)
    {
        DrawPrefix();
        Input.Draw(active);
        _previousLength = Input.Length;
    }

    public GuiInputResult ProcessInput(in ConsoleKeyInfo key)
    {
        var result = Input.ProcessInput(key);
        var (left, top) = Console.GetCursorPosition();
        DrawPrefix();
        _previousLength = Input.Length;
        Console.SetCursorPosition(left, top);
        return result;
    }

    private void DrawPrefix()
    {
        Console.SetCursorPosition(Console.WindowWidth - Input.Length - _prefix.Length, Input.Row);
        if (_previousLength >= Input.Length)
        {
            Console.CursorLeft--;
            Console.Write(' ');
        }

        Console.Write(_prefix);
    }

}
