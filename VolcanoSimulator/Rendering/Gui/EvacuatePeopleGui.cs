using System.Diagnostics.CodeAnalysis;
using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class EvacuatePeopleGui : GuiBase, IActionModeModifier
{

    private const string Title = "Evacuate from ";

    private readonly PrefixedInput<IntInputField> _people = new(new IntInputField(5, 2), "Group size: ");

    public IEvacuationLocation Origin { get; }

    public ActionMode Mode => ActionMode.Rescue;

    public override bool AllowIndicators => true;

    public string? PrimaryAction { get; private set; }

    public EvacuatePeopleGui(IEvacuationLocation origin) => Origin = origin;

    public override void Draw(SimulatorRenderer renderer)
    {
        Console.CursorVisible = true;
        Render.TextRight(4, Title, Origin is NamedLandmark {Name: var name} ? name : Origin.GetType().Name);
        _people.Draw(true);
        PrimaryAction = CanRescue(renderer, _people.Input.Value, out _) ? "[ENTER] Rescue to here" : null;
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key) => key.Key switch
    {
        ConsoleKey.W or ConsoleKey.A or ConsoleKey.S or ConsoleKey.D => GuiInputResult.Passthrough,
        ConsoleKey.Enter => BeginRescue(renderer),
        _ => _people.ProcessInput(key)
    };

    private GuiInputResult BeginRescue(SimulatorRenderer renderer)
    {
        var people = _people.Input.Value;
        if (people == 0)
        {
            Console.CursorVisible = false;
            return GuiInputResult.Exit;
        }

        if (Origin.AccommodatedPeople < people || !CanRescue(renderer, people, out var location))
            return GuiInputResult.None;
        Origin.Remove(people);
        renderer.Session.RegisterSurvivorGroup(new SurvivorGroup
        {
            Location = Origin.Location,
            AccommodatedPeople = people,
            Target = location
        });
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

    private bool CanRescue(SimulatorRenderer renderer, int people, [NotNullWhen(true)] out IEvacuationLocation? location)
    {
        if (renderer.SelectedLandmark is not IEvacuationLocation evacuationLocation)
        {
            location = null;
            return false;
        }

        location = evacuationLocation;
        return location != Origin && location.Capacity >= location.AccommodatedPeople + people;
    }

}
