namespace VolcanoSimulator.Rendering.Gui.Inputs;

public interface IInputField
{

    GuiInputResult ProcessInput(in ConsoleKeyInfo key);

}
