using VolcanoSimulator.Rendering.Gui;

namespace VolcanoSimulator.Rendering;

public sealed class SimulatorInput
{

    private readonly SimulatorRenderer _renderer;

    public SimulatorInput(SimulatorRenderer renderer) => _renderer = renderer;

    public bool Process(in ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.Escape && _renderer.CurrentGui == null)
            return false;
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
            case ConsoleKey.Escape or ConsoleKey.X:
                _renderer.ShowGui(new MenuGui());
                return true;
            case ConsoleKey.C when _renderer.SelectedLandmark == null:
                _renderer.ShowGui(new PlaceCityGui());
                break;
            case ConsoleKey.E when _renderer.SelectedLandmark == null:
                _renderer.ShowGui(new PlaceShelterGui());
                break;
            case ConsoleKey.V when _renderer.SelectedLandmark == null:
                _renderer.ShowGui(new PlaceVolcanoGui());
                break;
            case ConsoleKey.Delete:
                if (!_renderer.TryClearSelectedLandmark())
                    break;
                _renderer.Session.Landmarks.Remove(_renderer.SelectedLandmark!);
                _renderer.SelectedLandmark = null;
                break;
            default:
                if (key.Key.TryGetMovementDelta(out var delta)) _renderer.Move(key.IsShift() ? delta * 10 : delta);
                break;
        }

        return true;
    }

}
