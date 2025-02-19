using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering;

public sealed class PopulateCityGui : GuiBase, IActionModeModifier
{

    private const string Title = "Add people to ";

    private readonly City _city;

    private readonly PrefixedInput<IntInputField> _input = new(new IntInputField(1, PlaceCityGui.PopulationMaxLength), "Citizens: ");

    public ActionMode Mode => ActionMode.Insert;

    public PopulateCityGui(City city) => _city = city;

    public override void Draw(SimulatorRenderer renderer)
    {
        Console.CursorVisible = true;
        Render.TextRight(0, Title, _city.Name);
        _input.Draw(true);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key != ConsoleKey.Enter)
        {
            _input.ProcessInput(key);
            return GuiInputResult.None;
        }

        if (_input.Input.Value == 0)
            return GuiInputResult.Exit;
        _city.Shelter(_input.Input.Value);
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

}
