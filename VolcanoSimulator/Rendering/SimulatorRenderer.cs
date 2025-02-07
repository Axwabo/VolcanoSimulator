using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation = new(-Console.BufferHeight / 2, -Console.BufferWidth / 2);

    public SimulatorSession Session { get; }

    private ViewportRect Viewport => new(Console.BufferWidth, Console.BufferHeight, _viewLocation.Longitude, _viewLocation.Latitude);

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
            switch (landmark)
            {
                case City city:
                    new CityRenderer(city).Clear(viewport);
                    break;
            }
    }

    private void Draw()
    {
        var viewport = Viewport;
        foreach (var landmark in Session.Landmarks)
            switch (landmark)
            {
                case City city:
                    new CityRenderer(city).Draw(viewport);
                    break;
            }

        Console.SetCursorPosition(Console.BufferWidth / 2, Console.BufferHeight / 2);
        Console.Write('.');
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation = new Coordinates(_viewLocation.Latitude + delta.Latitude, _viewLocation.Longitude + delta.Longitude);
        Draw();
    }

}
