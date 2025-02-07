namespace VolcanoSimulator.Rendering.Gui;

public abstract class GuiBase
{

    public GuiBase? Parent { get; protected set; }

    public abstract void Draw();

    public abstract bool ProcessInput(in ConsoleKeyInfo key);

}
