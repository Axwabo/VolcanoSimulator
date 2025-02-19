using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class SimulationStepGui : GuiBase, IActionModeModifier
{

    private const string Padding = "   ";

    private static readonly TimeSpan MinTimeStep = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MaxTimeStep = TimeSpan.FromDays(1);

    private TimeSpan _step = TimeSpan.FromMinutes(1);

    private MaterialLayer _layers;

    public SimulationStepGui(MaterialLayer layers) => _layers = layers;

    public override bool AllowIndicators => true;

    public ActionMode Mode => ActionMode.Step;

    public override void Draw()
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 2);
        Console.Write("Time step: ");
        Console.Write(_step);
        Console.SetCursorPosition(Render.ModeWidth + Render.ModePrefix.Length + Render.ModeSuffix.Length, Console.WindowHeight - 1);
        Console.Write(Padding);
        SetColor(MaterialLayer.Lava);
        Console.Write("[L]ava");
        Console.ResetColor();
        Console.Write(Padding);
        SetColor(MaterialLayer.AshCloud);
        Console.Write("[C]louds");
        Console.ResetColor();
    }

    private void SetColor(MaterialLayer highlighted) => Console.ForegroundColor = _layers.HasFlagFast(highlighted) ? ConsoleColor.White : ConsoleColor.DarkGray;

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key) => key.Key switch
    {
        ConsoleKey.Escape => ShowMenu(renderer),
        ConsoleKey.Enter => Step(renderer),
        ConsoleKey.Spacebar => AdjustStep(),
        ConsoleKey.L => ToggleLayer(renderer, MaterialLayer.Lava),
        ConsoleKey.C => ToggleLayer(renderer, MaterialLayer.AshCloud),
        ConsoleKey.E or ConsoleKey.V or ConsoleKey.Delete => GuiInputResult.None,
        _ => GuiInputResult.Passthrough
    };

    private GuiInputResult ShowMenu(SimulatorRenderer renderer)
    {
        renderer.ShowGui(new MenuGui {Parent = this});
        return GuiInputResult.None;
    }

    private GuiInputResult Step(SimulatorRenderer renderer)
    {
        renderer.Session.Step(_step);
        return renderer.Session.AnyActive ? GuiInputResult.FullRedraw : GuiInputResult.Exit;
    }

    private GuiInputResult AdjustStep()
    {
        _step *= 10;
        if (_step > MaxTimeStep)
            _step = MinTimeStep;
        Draw();
        return GuiInputResult.None;
    }

    private GuiInputResult ToggleLayer(SimulatorRenderer renderer, MaterialLayer layer)
    {
        _layers ^= layer;
        renderer.Layers = _layers;
        return GuiInputResult.FullRedraw;
    }

}
