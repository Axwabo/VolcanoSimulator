namespace VolcanoSimulator.Rendering.Gui;

public abstract class PlaceLandmarkGuiBase : GuiBase, IActionModeModifier
{

    public ActionMode Mode => ActionMode.Insert;

    protected static Coordinates GetPlaceLocation(in ViewportRect viewport) => new(viewport.Y + viewport.Height / 2, viewport.X + viewport.Width / 2);

    protected static GuiInputResult Exit()
    {
        Console.CursorVisible = false;
        return GuiInputResult.Exit;
    }

}
