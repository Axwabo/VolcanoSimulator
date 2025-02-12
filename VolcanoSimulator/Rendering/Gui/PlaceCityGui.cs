using System.Buffers;
using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceCityGui : GuiBase
{

    private const string Title = "Place city:";

    private char[] _name = ArrayPool<char>.Shared.Rent(64);

    private int _length;

    private bool _editingName = Console.CursorVisible = true;

    public override void Draw()
    {
        Render.TextRight(0, Title);
        WriteName();
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
            return ToggleNameField(renderer, key);
        if (key.Key == ConsoleKey.Escape)
        {
            Console.CursorVisible = false;
            return GuiInputResult.Exit;
        }

        if (!_editingName)
            return key.Key.TryGetMovementDelta(out _) ? GuiInputResult.Passthrough : GuiInputResult.None;
        if (key.Key != ConsoleKey.Backspace)
        {
            if (_length == _name.Length)
                return GuiInputResult.None;
            ClearName();
            _name[_length++] = key.KeyChar;
            WriteName();
            return GuiInputResult.None;
        }

        if (_length == 0)
            return GuiInputResult.None;
        Console.SetCursorPosition(Console.WindowWidth - _length, 1);
        Console.Write(' ');
        _length--;
        WriteName();
        return GuiInputResult.None;
    }

    private GuiInputResult ToggleNameField(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (_length == 0 || !key.IsControl())
        {
            Console.CursorVisible = _editingName = !_editingName;
            return GuiInputResult.None;
        }

        var viewport = renderer.Viewport;
        renderer.Session.Landmarks.Add(new City
        {
            Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
            Name = _name.AsSpan()[.._length].ToString()
        });
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

    private void ClearName() => Erase.TextRight(1, _length);

    private void WriteName()
    {
        Render.TextRight(1, _name.AsSpan()[.._length]);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
    }

    ~PlaceCityGui() => ArrayPool<char>.Shared.Return(_name);

}
