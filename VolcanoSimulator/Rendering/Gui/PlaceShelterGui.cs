using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceShelterGui : GuiBase
{

    private const string Title = "Place evacuation shelter";

    private readonly PrefixedInput<IntInputField> _capacity = new(new IntInputField(1, 8), "Capacity: ");

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
        var value = _capacity.Input.Value;
        if (result != GuiInputResult.Exit || value == 0)
            return GuiInputResult.None;
        var viewport = renderer.Viewport;
        var shelter = new EvacuationShelter
        {
            Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
            ShelterCapacity = value
        };
        renderer.Session.Landmarks.Add(shelter);
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

}
