using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation = new(-Console.WindowHeight / 2, -Console.WindowWidth / 2);

    public SimulatorSession Session { get; }

    public ViewportRect Viewport => new(Console.WindowWidth, Console.WindowHeight, _viewLocation.Longitude, _viewLocation.Latitude);

    private GuiBase? _currentGui;

    private LandmarkBase? _selectedLandmark;

    public SimulatorRenderer(SimulatorSession session) => Session = session;

    public void RedrawAll()
    {
        Console.Clear();
        Draw();
    }

    private void Clear()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        Erase.SelectionIndicator(center);
        if (_selectedLandmark != null)
        {
            _selectedLandmark.ClearInfo();
            _selectedLandmark = null;
        }

        Session.Landmarks.ClearAll(viewport);
    }

    private void Draw()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        if (_currentGui == null && Session.Landmarks.DrawAllAndTryGetSelected(viewport, center, out _selectedLandmark))
        {
            Render.SelectionIndicator(center);
            _selectedLandmark.DrawInfo();
        }
        else
        {
            Render.Cursor = center;
            Console.Write('+');
            _currentGui?.Draw();
        }
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation += delta;
        Draw();
    }

    public bool ProcessInput(in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape && _currentGui == null)
            return false;
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
            case ConsoleKey.C when _selectedLandmark == null:
                ShowGui(new PlaceCityGui());
                break;
            case ConsoleKey.Delete:
                if (_selectedLandmark == null)
                    break;
                var center = Viewport.Size / 2;
                Erase.SelectionIndicator(center);
                _selectedLandmark.ClearInfo();
                Session.Landmarks.Remove(_selectedLandmark);
                Render.Cursor = center;
                Console.Write('+');
                break;
            default:
                if (key.Key.TryGetMovementDelta(out var delta))
                    Move(key.IsShift() ? delta * 10 : delta);
                break;
        }

        return true;
    }

    private void ShowGui(PlaceCityGui gui)
    {
        var current = _currentGui;
        _currentGui = gui;
        if (current != null)
        {
            RedrawAll();
            return;
        }

        _selectedLandmark?.ClearInfo();
        gui.Draw();
    }

}
