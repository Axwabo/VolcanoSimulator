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
        ClearSelectedLandmark(viewport);
        Session.Landmarks.ClearAll(viewport);
    }

    private void ClearSelectedLandmark(in ViewportRect viewport)
    {
        if (_selectedLandmark == null)
            return;
        var center = new Coordinates(viewport.Height / 2, viewport.Width / 2);
        Render.SelectionIndicator(center, false);
        _selectedLandmark.ClearInfo(viewport);
        _selectedLandmark = null;
    }

    private void Draw()
    {
        var viewport = Viewport;
        var center = new Coordinates(viewport.Height / 2, viewport.Width / 2);
        if (Session.Landmarks.DrawAllAndTryGetSelected(viewport, center, out _selectedLandmark))
        {
            Render.SelectionIndicator(center, true);
            _selectedLandmark.DrawInfo();
        }

        Render.Cursor = center;
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
            case ConsoleKey.C:
                _currentGui = new PlaceCityGui();
                RedrawAll();
                break;
            default:
                if (key.Key.TryGetMovementDelta(out var delta))
                    Move(delta);
                break;
        }

        return true;
    }

}
