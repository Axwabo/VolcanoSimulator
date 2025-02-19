using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceShelterGui : PlaceLandmarkGuiBase
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
            return Exit();
        var result = _capacity.ProcessInput(key);
        var value = _capacity.Input.Value;
        if (result != GuiInputResult.Exit || value == 0)
            return GuiInputResult.None;
        var shelter = new EvacuationShelter
        {
            Location = GetPlaceLocation(renderer.Viewport),
            ShelterCapacity = value
        };
        renderer.Session.Landmarks.Add(shelter);
        return Exit();
    }

}
