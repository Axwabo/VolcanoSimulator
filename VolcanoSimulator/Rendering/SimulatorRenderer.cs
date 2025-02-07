using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private static readonly Coordinates Up = new(-1, 0);
    private static readonly Coordinates Down = new(1, 0);
    private static readonly Coordinates Left = new(0, -1);
    private static readonly Coordinates Right = new(0, 1);

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
        Console.Write('+');
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation = new Coordinates(_viewLocation.Latitude + delta.Latitude, _viewLocation.Longitude + delta.Longitude);
        Draw();
    }

    public bool ProcessInput(in ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Escape or ConsoleKey.X:
                return false;
            case ConsoleKey.UpArrow:
                Move(Up);
                break;
            case ConsoleKey.DownArrow:
                Move(Down);
                break;
            case ConsoleKey.LeftArrow:
                Move(Left);
                break;
            case ConsoleKey.RightArrow:
                Move(Right);
                break;
        }

        return true;
    }

}
