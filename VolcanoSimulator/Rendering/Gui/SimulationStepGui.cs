using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class SimulationStepGui : GuiBase, IActionModeModifier
{

    private const string TimeStep = "Time step: ";
    private const string Padding = "   ";
    private const string People = "People: ";

    private static readonly TimeSpan[] TimeStepPresets =
    [
        TimeSpan.FromSeconds(1),
        TimeSpan.FromSeconds(10),
        TimeSpan.FromMinutes(1),
        TimeSpan.FromMinutes(5),
        TimeSpan.FromMinutes(10),
        TimeSpan.FromMinutes(30),
        TimeSpan.FromHours(1),
        TimeSpan.FromHours(6),
        TimeSpan.FromHours(12)
    ];

    // HH:mm:ss
    private readonly char[] _stepInput = new char[8];

    private int _stepInputPosition;

    private TimeSpan _step;

    private MaterialLayer _layers;

    private SurvivorGroup? _highlightedGroup;

    private int _previousPeopleLength;

    public SimulationStepGui(MaterialLayer layers)
    {
        _layers = layers;
        _step = TimeSpan.FromMinutes(1);
        _step.TryFormat(_stepInput, out _);
    }

    public override bool AllowIndicators => true;

    public ActionMode Mode => ActionMode.Step;

    public string? PrimaryAction { get; private set; }

    public override void Draw(SimulatorRenderer renderer)
    {
        Console.CursorVisible = true;
        Console.SetCursorPosition(0, Console.WindowHeight - 2);
        Console.Write(TimeStep);
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
        PrimaryAction = renderer.SelectedLandmark is IEvacuationLocation {AccommodatedPeople: not 0} ? "[ENTER] Evacuate people" : "[SPACE] Step";
        if (_previousPeopleLength != 0)
        {
            Erase.TextRight(Console.WindowHeight - 3, nameof(SurvivorGroup).Length);
            Erase.TextRight(Console.WindowHeight - 2, People.Length + (int) Math.Log10(_previousPeopleLength) + 1);
            _previousPeopleLength = 0;
        }

        var viewport = renderer.Viewport;
        var center = viewport.Size / 2;
        _highlightedGroup = renderer.Session.SurvivorGroups.FirstOrDefault(e => e.AccommodatedPeople != 0 && viewport.TryTransform(e.Location, out var coordinates) && coordinates == center);
        if (_highlightedGroup != null)
        {
            Render.Cursor = center;
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write(' ');
            Console.ResetColor();
            Render.TextRight(Console.WindowHeight - 3, nameof(SurvivorGroup));
            var people = _highlightedGroup.AccommodatedPeople.ToString();
            _previousPeopleLength = people.Length;
            Render.TextRight(Console.WindowHeight - 2, People, people);
            PrimaryAction = "[ENTER] Change target shelter";
        }

        DisplayCursor();
    }

    private void SetColor(MaterialLayer highlighted) => Console.ForegroundColor = _layers.HasFlagFast(highlighted) ? ConsoleColor.White : ConsoleColor.DarkGray;

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key) => key.Key switch
    {
        ConsoleKey.Escape => ShowMenu(renderer),
        ConsoleKey.Enter => Evacuate(renderer),
        ConsoleKey.Spacebar => Step(renderer),
        ConsoleKey.L => ToggleLayer(renderer, MaterialLayer.Lava),
        ConsoleKey.C => ToggleLayer(renderer, MaterialLayer.AshCloud),
        ConsoleKey.E or ConsoleKey.V or ConsoleKey.Delete => GuiInputResult.None,
        ConsoleKey.I => AdjustStep(renderer),
        ConsoleKey.LeftArrow => MoveCursor(-1),
        ConsoleKey.RightArrow => MoveCursor(1),
        ConsoleKey.W or ConsoleKey.A or ConsoleKey.S or ConsoleKey.D => GuiInputResult.Passthrough,
        _ => Type(key.KeyChar)
    };

    private GuiInputResult MoveCursor(int delta)
    {
        _stepInputPosition += delta;
        if (_stepInputPosition >= 0 && _stepInputPosition < _stepInput.Length && _stepInput[_stepInputPosition] == ':')
            _stepInputPosition += delta;
        if (_stepInputPosition < 0)
            _stepInputPosition = _stepInput.Length - 1;
        else if (_stepInputPosition >= _stepInput.Length)
            _stepInputPosition = 0;
        DisplayCursor();
        return GuiInputResult.None;
    }

    private void DisplayCursor() => Console.SetCursorPosition(TimeStep.Length + _stepInputPosition, Console.WindowHeight - 2);

    private GuiInputResult Type(char character)
    {
        if (!char.IsDigit(character))
            return GuiInputResult.None;
        var original = _stepInput[_stepInputPosition];
        _stepInput[_stepInputPosition] = character;
        if (!TimeSpan.TryParse(_stepInput, out var step))
        {
            _stepInput[_stepInputPosition] = original;
            return GuiInputResult.None;
        }

        _step = step;
        Console.Write(character);
        MoveCursor(1);
        return GuiInputResult.None;
    }

    private GuiInputResult ShowMenu(SimulatorRenderer renderer)
    {
        renderer.ShowGui(new MenuGui {Parent = this});
        return GuiInputResult.None;
    }

    private GuiInputResult Evacuate(SimulatorRenderer renderer)
    {
        if (_highlightedGroup != null)
        {
            renderer.ShowGui(new ChangeEvacuationTargetGui(_highlightedGroup) {Parent = this});
            return GuiInputResult.None;
        }

        if (renderer.SelectedLandmark is not IEvacuationLocation {AccommodatedPeople: not 0} location)
            return GuiInputResult.None;
        renderer.ShowGui(new EvacuatePeopleGui(location) {Parent = this});
        return GuiInputResult.None;
    }

    private GuiInputResult Step(SimulatorRenderer renderer)
    {
        if (_step <= TimeSpan.Zero)
            return GuiInputResult.None;
        renderer.Session.Step(_step);
        if (renderer.Session.AnyActive)
            return GuiInputResult.FullRedraw;
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

    private GuiInputResult AdjustStep(SimulatorRenderer renderer)
    {
        var increased = false;
        foreach (var timeStep in TimeStepPresets)
        {
            if (timeStep <= _step)
                continue;
            _step = timeStep;
            increased = true;
            break;
        }

        if (!increased)
            _step = TimeStepPresets[0];

        Draw(renderer);
        return GuiInputResult.None;
    }

    private GuiInputResult ToggleLayer(SimulatorRenderer renderer, MaterialLayer layer)
    {
        _layers ^= layer;
        renderer.Layers = _layers;
        return GuiInputResult.FullRedraw;
    }

}
