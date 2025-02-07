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

    private bool _editingName;

    public override void Draw()
    {
        Write(Title, 0);
        Write(_name, 1);
        Console.SetCursorPosition(Console.WindowWidth - 1, 1);
    }

    public override bool ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
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
                return false;
            }

            Console.CursorVisible = _editingName = !_editingName;
            return true;
        }

        if (!_editingName)
            return false;
        if (key.Key != ConsoleKey.Backspace)
        {
            _name += key.KeyChar;
            return true;
        }

        if (_name.Length == 0)
            return false;
        _name = _name[..^1];
        return true;
    }

}
