namespace VolcanoSimulator.Rendering;

public interface IRenderable
{

    void Draw(ViewportRect viewport);

    void Clear(ViewportRect viewport);

}
