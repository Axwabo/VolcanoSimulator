using VolcanoSimulator.Rendering.Gui;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorInput
{

    private readonly SimulatorRenderer _renderer;

    public SimulatorInput(SimulatorRenderer renderer) => _renderer = renderer;

    public bool Process(in ConsoleKeyInfo key)
    {
        if (_renderer.CurrentGui == null)
            return HandleDefault(key);
        var result = _renderer.CurrentGui.ProcessInput(_renderer, key);
        switch (result)
        {
            case GuiInputResult.None:
                return true;
            case GuiInputResult.FullRedraw:
                _renderer.RedrawAll();
                return true;
            case GuiInputResult.Passthrough:
                return HandleDefault(key);
            case GuiInputResult.Exit:
                _renderer.ShowGui(_renderer.CurrentGui.Parent);
                return true;
            case GuiInputResult.QuitGame:
                return false;
            default:
                return HandleDefault(key);
        }
    }

    private bool HandleDefault(in ConsoleKeyInfo key)
    {
        switch (key.Key)
        {
            case ConsoleKey.Enter when _renderer.SelectedLandmark is Volcano volcano:
                _renderer.Session.RegisterEarthquake(volcano.Erupt());
                _renderer.ShowGui(new SimulationStepGui());
                return true;
            case ConsoleKey.Escape:
                _renderer.ShowGui(new MenuGui());
                return true;
            case ConsoleKey.Delete:
                if (!_renderer.TryClearSelectedLandmark())
                    return true;
                _renderer.Session.Landmarks.Remove(_renderer.SelectedLandmark!);
                _renderer.SelectedLandmark = null;
                return true;
            default:
                if (_renderer.SelectedLandmark == null && ShowAddGui(key.Key))
                    return true;
                if (key.Key.TryGetMovementDelta(out var delta))
                    _renderer.Move(key.IsShift() ? delta * 10 : delta);
                return true;
        }
    }

    private bool ShowAddGui(ConsoleKey key)
    {
        var gui = key switch
        {
            ConsoleKey.C => new PlaceCityGui(),
            ConsoleKey.E => new PlaceShelterGui(),
            ConsoleKey.V => new PlaceVolcanoGui(),
            _ => (GuiBase?) null
        };
        if (gui == null)
            return false;
        _renderer.ShowGui(gui);
        return true;
    }

}
