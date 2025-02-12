namespace VolcanoSimulator.Rendering.Gui.Inputs;

public class IntInputField : InputField
{

    public IntInputField(int row, int maxLength) : base(row, maxLength) => Append('0');

    public int Value => Length == 0 ? 0 : int.Parse(Text);

    protected override bool IsAllowed(char character) => char.IsDigit(character);

}
