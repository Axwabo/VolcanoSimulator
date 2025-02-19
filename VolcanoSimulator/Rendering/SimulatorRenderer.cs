using VolcanoSimulator.Rendering.Extensions;
using VolcanoSimulator.Rendering.Gui;
using VolcanoSimulator.Rendering.Renderers;
using VolcanoSimulator.Simulation;
using VolcanoSimulator.Simulation.Materials;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorRenderer
{

    private Coordinates _viewLocation = new(-Console.WindowHeight / 2, -Console.WindowWidth / 2);

    private int _previousLocationLength;
    private int _previousEarthquakeLength;
    private MaterialLayer _previousLayers;
    private ActionMode _previousMode;

    public SimulatorSession Session { get; }

    public SimulatorInput Input { get; }

    public RendererTable CachedRenderers { get; } = [];

    public ViewportRect Viewport => new(Console.WindowWidth, Console.WindowHeight, _viewLocation.Longitude, _viewLocation.Latitude);

    public GuiBase? CurrentGui { get; set; }

    public LandmarkBase? SelectedLandmark { get; set; }

    public MaterialLayer Layers { get; set; } = MaterialLayer.AshCloud | MaterialLayer.Lava;

    public ActionMode Mode => (CurrentGui as IActionModeModifier)?.Mode ?? ActionMode.Normal;

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
        Erase.Mode(_previousMode);
        Erase.SimulatorLocation(_previousLocationLength, _previousEarthquakeLength);
        Erase.SelectionIndicator(center);
        if (SelectedLandmark != null)
        {
            SelectedLandmark.ClearInfo();
            SelectedLandmark = null;
        }

        Session.AllEruptedMaterials.ClearAll(CachedRenderers, viewport, _previousLayers);
        Session.Landmarks.ClearAll(CachedRenderers, viewport);
    }

    private void Draw()
    {
        var viewport = Viewport;
        var center = viewport.Size / 2;
        if (Session.Landmarks.DrawAllAndTryGetSelected(CachedRenderers, viewport, center, out var landmark) && CurrentGui is not {AllowIndicators: false})
        {
            Session.AllEruptedMaterials.DrawAll(CachedRenderers, viewport, Layers);
            Render.SelectionIndicator(center);
            landmark.DrawInfo();
            SelectedLandmark = landmark;
            if (CurrentGui is {AllowIndicators: true})
                CurrentGui.Draw();
        }
        else
        {
            Session.AllEruptedMaterials.DrawAll(CachedRenderers, viewport, Layers);
            Render.Cursor = center;
            Console.Write('+');
            CurrentGui?.Draw();
        }

        var strength = Session.GetTotalEarthquakeStrengthAt(_viewLocation + viewport.Size / 2);
        (_previousLocationLength, _previousEarthquakeLength) = Render.SimulatorInfo(viewport, _viewLocation, strength);
        _previousLayers = Layers;
        Render.Mode(_previousMode = Mode);
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
        Render.Mode(_previousMode = Mode);
        gui?.Draw();
    }

}
