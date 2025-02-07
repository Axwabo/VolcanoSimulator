namespace VolcanoSimulator.Rendering.Renderers;

public interface IRenderer
{

    void Draw(in ViewportRect viewport);

    void Clear(in ViewportRect viewport);

}
