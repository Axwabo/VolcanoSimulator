namespace VolcanoSimulator.Rendering.Renderers;

public sealed class SurvivorGroupRenderer : PositionedRenderer
{

    public SurvivorGroupRenderer(SurvivorGroup group) : base(group)
    {
    }

    public override void Draw(in ViewportRect viewport)
    {
        if (!SetPosition(viewport))
            return;
        Console.BackgroundColor = ConsoleColor.Green;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write('+');
        Console.ResetColor();
    }

    public override void Clear(in ViewportRect viewport)
    {
        if (SetPosition(viewport))
            Console.Write(' ');
    }

}
