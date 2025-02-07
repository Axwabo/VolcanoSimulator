using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private static readonly Coordinates Up = new(-1, 0);
    private static readonly Coordinates Down = new(1, 0);
    private static readonly Coordinates Left = new(0, -1);
    private static readonly Coordinates Right = new(0, 1);

    private Coordinates _viewLocation = new(-Console.WindowHeight / 2, -Console.WindowWidth / 2);

    public SimulatorSession Session { get; }

    public ViewportRect Viewport => new(Console.WindowWidth, Console.WindowHeight, _viewLocation.Longitude, _viewLocation.Latitude);

    private GuiBase? _currentGui;

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

        Console.SetCursorPosition(Console.WindowWidth / 2, Console.WindowHeight / 2);
        Console.Write('+');
        _currentGui?.Draw();
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation = new Coordinates(_viewLocation.Latitude + delta.Latitude, _viewLocation.Longitude + delta.Longitude);
        Draw();
    }

    public bool ProcessInput(in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape)
        {
            if (_currentGui == null)
                return false;
            _currentGui = _currentGui.Parent;
            RedrawAll();
            return true;
        }

        if (_currentGui == null)
            return HandleDefaultInput(key);
        var result = _currentGui.ProcessInput(this, key);
        switch (result)
        {
            case GuiInputResult.None:
                return true;
            case GuiInputResult.FullRedraw:
                RedrawAll();
                return true;
            case GuiInputResult.Passthrough:
                return HandleDefaultInput(key);
            case GuiInputResult.Exit:
                _currentGui = _currentGui.Parent;
                RedrawAll();
                return true;
            default:
                return HandleDefaultInput(key);
        }
    }

    private bool HandleDefaultInput(in ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Escape or ConsoleKey.X:
                return false;
            case ConsoleKey.C:
                _currentGui = new PlaceCityGui();
                RedrawAll();
                break;
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
