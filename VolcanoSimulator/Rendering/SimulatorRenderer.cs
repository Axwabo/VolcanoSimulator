using VolcanoSimulator.Models;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation;

    public SimulatorSession Session { get; }

    private ViewportRect Viewport
    {
        get
        {
            var w = Console.WindowWidth;
            var h = Console.WindowHeight;
            return new ViewportRect(w, h, _viewLocation.Longitude + w / 2, _viewLocation.Latitude + h / 2);
        }
    }

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
            GetRenderable(landmark).Clear(viewport);
    }

    private void Draw()
    {
        var viewport = Viewport;
        foreach (var landmark in Session.Landmarks)
            GetRenderable(landmark).Draw(viewport);
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation = new Coordinates(_viewLocation.Latitude + delta.Latitude, _viewLocation.Longitude + delta.Longitude);
        Draw();
    }

    private static IRenderable GetRenderable(LandmarkBase landmark) => landmark switch
    {
        _ => throw new ArgumentException($"Landmark {landmark} cannot be rendered", nameof(landmark))
    };

}
