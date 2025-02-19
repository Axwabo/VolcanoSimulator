namespace VolcanoSimulator.Rendering.Gui;

public abstract class GuiBase
{

    public GuiBase? Parent { get; init; }

    public virtual bool AllowIndicators => false;

    public abstract void Draw(SimulatorRenderer renderer);

    public abstract GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key);

}
