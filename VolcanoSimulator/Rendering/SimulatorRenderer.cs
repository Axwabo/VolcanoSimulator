using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private const string Name = "Name: ";
    private const string People = "People: ";

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
        foreach (var landmark in Session.Landmarks)
            switch (landmark)
            {
                case City city:
                    new CityRenderer(city).Clear(viewport);
                    break;
            }
    }

    private void ClearSelectedLandmark(in ViewportRect viewport)
    {
        if (_selectedLandmark == null)
            return;
        var centerX = viewport.Width / 2;
        var centerY = viewport.Height / 2;
        Console.SetCursorPosition(centerX - 1, centerY);
        Console.Write(' ');
        Console.SetCursorPosition(centerX + 1, centerY);
        Console.Write(' ');
        ClearLandmarkInfo(_selectedLandmark, viewport);
        _selectedLandmark = null;
    }

    private static void ClearLandmarkInfo(LandmarkBase landmark, in ViewportRect viewport)
    {
        if (landmark is City city)
        {
            var length = Name.Length + city.Name.Length;
            Console.SetCursorPosition(viewport.Width - length, 0);
            for (var i = 0; i < length; i++)
                Console.Write(' ');
        }

        if (landmark is IEvacuationLocation evacuationLocation)
        {
            var length = People.Length + IntLength(evacuationLocation.AccommodatedPeople);
            Console.SetCursorPosition(viewport.Width - length, 1);
            for (var i = 0; i < length; i++)
                Console.Write(' ');
        }
    }

    private static int IntLength(int value) => value == 0 ? 1 : (int) Math.Floor(Math.Log10(value)) + 1;

    private void Draw()
    {
        var viewport = Viewport;
        var centerX = viewport.Width / 2;
        var centerY = viewport.Height / 2;
        foreach (var landmark in Session.Landmarks)
        {
            switch (landmark)
            {
                case City city:
                    new CityRenderer(city).Draw(viewport);
                    break;
            }

            if (viewport.TryTransform(landmark.Location, out var screen) && screen.Longitude == centerX && screen.Latitude == centerY)
                _selectedLandmark = landmark;
        }

        if (_selectedLandmark != null)
        {
            Console.SetCursorPosition(centerX - 1, centerY);
            Console.Write('>');
            Console.SetCursorPosition(centerX + 1, centerY);
            Console.Write('<');
            DrawLandmarkInfo(_selectedLandmark);
        }

        Console.SetCursorPosition(centerX, centerY);
        Console.Write('+');
        _currentGui?.Draw();
    }

    private static void DrawLandmarkInfo(LandmarkBase landmark)
    {
        if (landmark is City city)
            Render.TextRight(0, Name, city.Name);
        if (landmark is IEvacuationLocation evacuationLocation)
            Render.TextRight(1, People, evacuationLocation.AccommodatedPeople.ToString());
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
