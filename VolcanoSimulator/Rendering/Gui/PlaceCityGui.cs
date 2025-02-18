using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceCityGui : GuiBase
{

    private const string Title = "Place city";

    private readonly PrefixedInput<InputField> _nameInput = new(new InputField(1, 64), "Name: ");
    private readonly PrefixedInput<IntInputField> _peopleInput = new(new IntInputField(2, 8), "Citizens: ");

    private bool _peopleActive;

    public override void Draw()
    {
        Console.CursorVisible = true;
        Render.TextRight(0, Title);
        _nameInput.Draw(!_peopleActive);
        _peopleInput.Draw(_peopleActive);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
        {
            Console.CursorVisible = false;
            return GuiInputResult.Exit;
        }

        if (key.Key is ConsoleKey.UpArrow or ConsoleKey.DownArrow)
        {
            _peopleActive = !_peopleActive;
            Console.SetCursorPosition(Console.WindowWidth - 1, _peopleActive ? 2 : 1);
            return GuiInputResult.None;
        }

        var result = (_peopleActive ? (IInputField) _peopleInput : _nameInput).ProcessInput(key);
        if (result != GuiInputResult.Exit)
            return GuiInputResult.None;
        var viewport = renderer.Viewport;
        var city = new City
        {
            Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
            Name = _nameInput.Input.Text.ToString()
        };
        var people = _peopleInput.Input.Value;
        if (people != 0)
            city.Shelter(people);
        renderer.Session.Landmarks.Add(city);
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

}
