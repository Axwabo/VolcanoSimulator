namespace VolcanoSimulator.Rendering.Renderers;

public abstract class PositionedRenderer
{

    public static Length PixelSize { get; } = Length.FromHectometers(1);

    protected IPositioned Positioned { get; }

    protected PositionedRenderer(IPositioned positioned) => Positioned = positioned;

    protected bool SetPosition(in ViewportRect viewport)
    {
        if (!viewport.TryTransform(Positioned.Location, out var screen))
            return false;
        Render.Cursor = screen;
        return true;
    }

    public abstract void Draw(in ViewportRect viewport);

    public abstract void Clear(in ViewportRect viewport);

}
