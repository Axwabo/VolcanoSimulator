namespace VolcanoSimulator.Rendering.Gui;

public sealed class SimulationStepGui : GuiBase
{

    private static readonly TimeSpan MinTimeStep = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan MaxTimeStep = TimeSpan.FromDays(1);

    private TimeSpan _step = TimeSpan.FromMinutes(1);

    public override bool AllowIndicators => true;

    public override void Draw()
    {
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write("Time step: ");
        Console.Write(_step);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key) => key.Key switch
    {
        ConsoleKey.Enter => Step(renderer),
        ConsoleKey.Escape => GuiInputResult.None,
        ConsoleKey.Spacebar => AdjustStep(),
        ConsoleKey.E or ConsoleKey.V or ConsoleKey.C => GuiInputResult.None,
        _ => GuiInputResult.Passthrough
    };

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

}
