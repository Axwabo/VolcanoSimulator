namespace VolcanoSimulator.Rendering.Gui.Inputs;

public interface IInputField
{

    void Draw(bool active);

    GuiInputResult ProcessInput(in ConsoleKeyInfo key);

}
