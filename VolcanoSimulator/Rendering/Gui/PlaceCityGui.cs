using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceCityGui : GuiBase
{

    private const string Title = "Place city:";

    private ArraySegment<char> _name = new(new char[64], 0, 0);

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
            return GuiInputResult.Passthrough;
        if (key.Key != ConsoleKey.Backspace)
        {
            if (_name.Count == _name.Array!.Length)
                return GuiInputResult.None;
            ClearName();
            _name = new ArraySegment<char>(_name.Array, _name.Offset, _name.Count + 1);
            _name.Array![_name.Count - 1] = key.KeyChar;
            WriteName();
            return GuiInputResult.None;
        }

        if (_name.Count == 0)
            return GuiInputResult.None;
        Console.SetCursorPosition(Console.WindowWidth - _name.Count, 1);
        Console.Write(' ');
        _name = _name[..^1];
        WriteName();
        return GuiInputResult.None;
    }

    private GuiInputResult ToggleNameField(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (_name.AsSpan().IsEmpty || !key.Modifiers.IsControl())
        {
            Console.CursorVisible = _editingName = !_editingName;
            return GuiInputResult.None;
        }

        var viewport = renderer.Viewport;
        renderer.Session.Landmarks.Add(new City
        {
            Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
            Name = _name.AsSpan().ToString()
        });
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

    private void ClearName() => Erase.TextRight(1, _name.Count);

    private void WriteName()
    {
        Render.TextRight(1, _name);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
    }

}
