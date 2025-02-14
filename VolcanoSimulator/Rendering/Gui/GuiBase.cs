namespace VolcanoSimulator.Rendering.Gui;

public abstract class GuiBase
{

    public GuiBase? Parent { get; init; }

    public abstract void Draw();

    public abstract GuiInputResult ProcessInput(SimulatorRenderer renderer, in ConsoleKeyInfo key);

}
