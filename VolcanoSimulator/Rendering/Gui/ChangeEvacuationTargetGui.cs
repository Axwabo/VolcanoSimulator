using System.Diagnostics.CodeAnalysis;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class ChangeEvacuationTargetGui : GuiBase, IActionModeModifier
{

    private const string Title = "Change target from ";

    public SurvivorGroup Group { get; }

    public ActionMode Mode => ActionMode.Rescue;

    public override bool AllowIndicators => true;

    public string? PrimaryAction { get; private set; }

    public ChangeEvacuationTargetGui(SurvivorGroup group) => Group = group;

    public override void Draw(SimulatorRenderer renderer)
    {
        Render.TextRight(4, Title, Group.Target is NamedLandmark {Name: var name} ? name : Group.GetType().Name);
        PrimaryAction = CanRescue(renderer, out _) ? "[ENTER] Rescue to here" : null;
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key) => key.Key switch
    {
        ConsoleKey.W or ConsoleKey.A or ConsoleKey.S or ConsoleKey.D => GuiInputResult.Passthrough,
        ConsoleKey.Enter => BeginRescue(renderer),
        ConsoleKey.Escape => GuiInputResult.Exit,
        _ => GuiInputResult.None
    };

    private GuiInputResult BeginRescue(SimulatorRenderer renderer)
    {
        if (!CanRescue(renderer, out var location))
            return GuiInputResult.None;
        Group.Target = location;
        return GuiInputResult.Exit;
    }

    private bool CanRescue(SimulatorRenderer renderer, [NotNullWhen(true)] out IEvacuationLocation? location)
    {
        if (renderer.SelectedLandmark is not IEvacuationLocation evacuationLocation)
        {
            location = null;
            return false;
        }

        location = evacuationLocation;
        return location != Group.Target;
    }

}
