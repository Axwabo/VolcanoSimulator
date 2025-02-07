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

    private const string Name = "Name: ";
    private const string People = "People: ";

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
        var centerX = viewport.Width / 2;
        var centerY = viewport.Height / 2;
        foreach (var landmark in Session.Landmarks)
        {
            switch (landmark)
            {
                case City city:
                    new CityRenderer(city).Clear(viewport);
                    break;
            }

            if (!viewport.TryTransform(landmark.Location, out var screen) || screen.Longitude != centerX || screen.Latitude != centerY)
                break;
            Console.SetCursorPosition(screen.Longitude - 1, screen.Latitude);
            Console.Write(' ');
            Console.SetCursorPosition(screen.Longitude + 1, screen.Latitude);
            Console.Write(' ');
            ClearLandmarkInfo(landmark, viewport);
        }
    }

    private void ClearLandmarkInfo(LandmarkBase landmark, in ViewportRect viewport)
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
        var centerX = Console.WindowWidth / 2;
        var centerY = Console.WindowHeight / 2;
        foreach (var landmark in Session.Landmarks)
        {
            switch (landmark)
            {
                case City city:
                    new CityRenderer(city).Draw(viewport);

                    break;
            }

            if (!viewport.TryTransform(landmark.Location, out var screen) || screen.Longitude != centerX || screen.Latitude != centerY)
                break;
            Console.SetCursorPosition(screen.Longitude - 1, screen.Latitude);
            Console.Write('>');
            Console.SetCursorPosition(screen.Longitude + 1, screen.Latitude);
            Console.Write('<');
            DrawLandmarkInfo(landmark, viewport);
        }

        Console.SetCursorPosition(centerX, centerY);
        Console.Write('+');
        _currentGui?.Draw();
    }

    private void DrawLandmarkInfo(LandmarkBase landmark, in ViewportRect viewport)
    {
        if (landmark is City city)
        {
            Console.SetCursorPosition(viewport.Width - Name.Length - city.Name.Length, 0);
            Console.Write(Name);
            Console.Write(city.Name);
        }

        if (landmark is IEvacuationLocation evacuationLocation)
        {
            var people = evacuationLocation.AccommodatedPeople.ToString();
            Console.SetCursorPosition(viewport.Width - People.Length - people.Length, 1);
            Console.Write(People);
            Console.Write(people);
        }
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
