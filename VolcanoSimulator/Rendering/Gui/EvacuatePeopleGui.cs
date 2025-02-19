﻿using VolcanoSimulator.Rendering.Gui.Inputs;

namespace VolcanoSimulator.Rendering.Gui;

public sealed class EvacuatePeopleGui : GuiBase, IActionModeModifier
{

    private const string Title = "Evacuate from ";

    private readonly PrefixedInput<IntInputField> _people = new(new IntInputField(1, 2), "Group size: ");

    public IEvacuationLocation Origin { get; }

    public ActionMode Mode => ActionMode.Rescue;

    public override bool AllowIndicators => true;

    public EvacuatePeopleGui(IEvacuationLocation origin) => Origin = origin;

    public override void Draw(SimulatorRenderer renderer)
    {
        Render.TextRight(0, Title, Origin is NamedLandmark {Name: var name} ? name : Origin.GetType().Name);
        _people.Draw(true);
    }

    public override GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key) => key.Key switch
    {
        ConsoleKey.W or ConsoleKey.A or ConsoleKey.S or ConsoleKey.D => GuiInputResult.Passthrough,
        _ => GuiInputResult.None
    };

}
