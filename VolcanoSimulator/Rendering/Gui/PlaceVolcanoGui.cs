using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceVolcanoGui : GuiBase
{

    private const string Title = "Place volcano";

    private readonly PrefixedInput<InputField> _capacity = new(new InputField(1, 32), "Name: ");

    public override void Draw()
    {
        Console.CursorVisible = true;
        Render.TextRight(0, Title);
        _capacity.Draw(true);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
        {
            Console.CursorVisible = false;
            return GuiInputResult.Exit;
        }

        var result = _capacity.ProcessInput(key);
        var value = _capacity.Input.Text;
        if (result != GuiInputResult.Exit || value.IsEmpty)
            return GuiInputResult.None;
        var viewport = renderer.Viewport;
        var shelter = new Volcano
        {
            Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
            Name = value.ToString()
        };
        renderer.Session.Landmarks.Add(shelter);
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

}
