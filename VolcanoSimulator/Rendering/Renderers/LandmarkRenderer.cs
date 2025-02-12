using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public abstract class LandmarkRenderer
{

    public LandmarkBase Landmark { get; }

    protected LandmarkRenderer(LandmarkBase landmark) => Landmark = landmark;

    protected bool SetPosition(in ViewportRect viewport)
    {
        if (!viewport.TryTransform(Landmark.Location, out var screen))
            return false;
        Render.Cursor = screen;
        return true;
    }

    public abstract void Draw(in ViewportRect viewport);

    public abstract void Clear(in ViewportRect viewport);

}
