using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceCityGui : GuiBase
{

    private const string Title = "Place city:";

    private static void Write(string text, int row)
    {
        if (text.Length == 0)
            return;
        Console.SetCursorPosition(Console.WindowWidth - text.Length, row);
        Console.Write(text);
    }

    private string _name = "";

    private bool _editingName = Console.CursorVisible = true;

    public override void Draw()
    {
        Write(Title, 0);
        Write(_name, 1);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Enter)
        {
            if (!string.IsNullOrWhiteSpace(_name) && (key.Modifiers & ConsoleModifiers.Control) != 0)
            {
                var viewport = renderer.Viewport;
                renderer.Session.Landmarks.Add(new City
                {
                    Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
                    Name = _name
                });
                Console.CursorVisible = false;
                return GuiInputResult.Exit;
            }

            Console.CursorVisible = _editingName = !_editingName;
            return GuiInputResult.None;
        }

        if (!_editingName)
            return GuiInputResult.Passthrough;
        if (key.Key != ConsoleKey.Backspace)
        {
            ClearName();
            _name += key.KeyChar;
            Write(_name, 1);
            Console.SetCursorPosition(Console.WindowWidth - 1, 1);
            return GuiInputResult.None;
        }

        if (_name.Length == 0)
            return GuiInputResult.None;
        Console.SetCursorPosition(Console.WindowWidth - _name.Length, 1);
        Console.WriteLine(' ');
        _name = _name[..^1];
        Write(_name, 1);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
        return GuiInputResult.None;
    }

    private void ClearName()
    {
        if (_name.Length == 0)
            return;
        Console.SetCursorPosition(Console.WindowWidth - _name.Length, 1);
        for (var i = 0; i < _name.Length; i++)
            Console.Write(' ');
    }

}
