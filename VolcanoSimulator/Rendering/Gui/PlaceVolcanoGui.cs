using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceVolcanoGui : PlaceLandmarkGuiBase
{

    private const string Title = "Place volcano";

    private readonly PrefixedInput<InputField> _nameInput = new(new InputField(1, 32), "Name: ");
    private readonly PrefixedInput<IntInputField> _indexInput = new(new IntInputField(2, 1), "VEI: ");

    private bool _indexActive;

    public override void Draw(SimulatorRenderer renderer)
    {
        Console.CursorVisible = true;
        Render.TextRight(0, Title);
        _nameInput.Draw(!_indexActive);
        _indexInput.Draw(_indexActive);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
            return Exit();

        if (key.Key is ConsoleKey.UpArrow or ConsoleKey.DownArrow)
        {
            _indexActive = !_indexActive;
            Console.SetCursorPosition(Console.WindowWidth - 1, _indexActive ? 2 : 1);
            return GuiInputResult.None;
        }

        var result = (_indexActive ? (IInputField) _indexInput : _nameInput).ProcessInput(key);
        var name = _nameInput.Input.Text;
        var index = _indexInput.Input.Value;
        if (result != GuiInputResult.Exit || name.IsEmpty || index > VolcanicExplosivityIndex.MaxIndex)
            return GuiInputResult.None;
        var shelter = new Volcano
        {
            Location = GetPlaceLocation(renderer.Viewport),
            Name = name.ToString(),
            ExplosivityIndex = new VolcanicExplosivityIndex((byte) index)
        };
        renderer.Session.Landmarks.Add(shelter);
        return Exit();
    }

}
