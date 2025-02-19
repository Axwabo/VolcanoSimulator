namespace VolcanoSimulator.Rendering.Renderers;

public class SymbolBasedRenderer : LandmarkRenderer
{

    public char Symbol { get; }

    protected SymbolBasedRenderer(LandmarkBase landmark, char symbol) : base(landmark) => Symbol = symbol;

    public override void Draw(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write(Symbol);
    }

    public override void Clear(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write(' ');
    }

}
