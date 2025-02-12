using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class PlaceCityGui : GuiBase
{

    private const string Title = "Place city:";

    private readonly InputField _nameInput = new(1, 64);

    public override void Draw()
    {
        Console.CursorVisible = true;
        Render.TextRight(0, Title);
        _nameInput.Draw();
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
        {
            Console.CursorVisible = false;
            return GuiInputResult.Exit;
        }

        var result = _nameInput.ProcessInput(key);
        if (result != GuiInputResult.Exit)
            return GuiInputResult.None;
        var viewport = renderer.Viewport;
        renderer.Session.Landmarks.Add(new City
        {
            Location = new Coordinates(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2),
            Name = _nameInput.Text.ToString()
        });
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

}
