using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceCityGui : PlaceLandmarkGuiBase
{

    private const string Title = "Place city";
    public const int PopulationMaxLength = 8;

    private readonly PrefixedInput<InputField> _nameInput = new(new InputField(1, 64), "Name: ");
    private readonly PrefixedInput<IntInputField> _peopleInput = new(new IntInputField(2, PopulationMaxLength), "Citizens: ");

    private bool _peopleActive;

    public override void Draw(SimulatorRenderer renderer)
    {
        Console.CursorVisible = true;
        Render.TextRight(0, Title);
        _nameInput.Draw(!_peopleActive);
        _peopleInput.Draw(_peopleActive);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
            return Exit();
        if (key.Key is ConsoleKey.UpArrow or ConsoleKey.DownArrow)
        {
            _peopleActive = !_peopleActive;
            Console.SetCursorPosition(Console.WindowWidth - 1, _peopleActive ? 2 : 1);
            return GuiInputResult.None;
        }

        var result = (_peopleActive ? (IInputField) _peopleInput : _nameInput).ProcessInput(key);
        if (result != GuiInputResult.Exit)
            return GuiInputResult.None;
        var city = new City
        {
            Location = GetPlaceLocation(renderer.Viewport),
            Name = _nameInput.Input.Text.ToString()
        };
        var people = _peopleInput.Input.Value;
        if (people != 0)
            city.Shelter(people);
        renderer.Session.Landmarks.Add(city);
        return Exit();
    }

}
