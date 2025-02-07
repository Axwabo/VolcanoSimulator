using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation = new(-Console.WindowHeight / 2, -Console.WindowWidth / 2);

    public SimulatorSession Session { get; }

    private ViewportRect Viewport => new(Console.WindowWidth, Console.WindowHeight, _viewLocation.Longitude, _viewLocation.Latitude);

    public SimulatorRenderer(SimulatorSession session) => Session = session;

    public void RedrawAll()
    {
        Console.Clear();
        Draw();
    }

    private void Clear()
    {
        var viewport = Viewport;
        foreach (var landmark in Session.Landmarks)
            IRenderer.GetRenderable(landmark).Clear(viewport);
    }

    private void Draw()
    {
        var viewport = Viewport;
        foreach (var landmark in Session.Landmarks)
            IRenderer.GetRenderable(landmark).Draw(viewport);
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation = new Coordinates(_viewLocation.Latitude + delta.Latitude, _viewLocation.Longitude + delta.Longitude);
        Draw();
    }

}
