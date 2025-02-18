using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Simulation;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation = new(-Console.WindowHeight / 2, -Console.WindowWidth / 2);

    private int _previousLocationLength;

    public SimulatorSession Session { get; }

    public SimulatorInput Input { get; }

    public RendererTable CachedRenderers { get; } = [];

    public ViewportRect Viewport => new(Console.WindowWidth, Console.WindowHeight, _viewLocation.Longitude, _viewLocation.Latitude);

    public GuiBase? CurrentGui { get; set; }

    public LandmarkBase? SelectedLandmark { get; set; }

    public SimulatorRenderer(SimulatorSession session)
    {
        Session = session;
        Input = new SimulatorInput(this);
    }

    public void RedrawAll()
    {
        Console.Clear();
        Draw();
    }

    private void Clear()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        Erase.SimulatorLocation(_previousLocationLength);
        Erase.SelectionIndicator(center);
        if (SelectedLandmark != null)
        {
            SelectedLandmark.ClearInfo();
            SelectedLandmark = null;
        }

        Session.AllEruptedMaterials.ClearAll(CachedRenderers, viewport);
        Session.Landmarks.ClearAll(CachedRenderers, viewport);
    }

    private void Draw()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        if (Session.Landmarks.DrawAllAndTryGetSelected(CachedRenderers, viewport, center, out var landmark) && CurrentGui is not {AllowIndicators: false})
        {
            Session.AllEruptedMaterials.DrawAll(CachedRenderers, viewport);
            Render.SelectionIndicator(center);
            landmark.DrawInfo();
            SelectedLandmark = landmark;
            if (CurrentGui is {AllowIndicators: true})
                CurrentGui.Draw();
        }
        else
        {
            Session.AllEruptedMaterials.DrawAll(CachedRenderers, viewport);
            Render.Cursor = center;
            Console.Write('+');
            CurrentGui?.Draw();
        }

        _previousLocationLength = Render.SimulatorInfo(viewport, _viewLocation);
    }

    public void Move(Coordinates delta)
    {
        Clear();
        _viewLocation += delta;
        Draw();
    }

    public void ShowGui(GuiBase? gui)
    {
        var current = CurrentGui;
        CurrentGui = gui;
        if (current != null)
        {
            RedrawAll();
            return;
        }

        if (gui is {AllowIndicators: false})
            SelectedLandmark?.ClearInfo();
        gui?.Draw();
    }

}
