using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public interface IRenderer
{

    void Draw(in ViewportRect viewport);

    void Clear(in ViewportRect viewport);

    public static IRenderer GetRenderable(LandmarkBase landmark) => landmark switch
    {
        City city => new CityRenderer(city),
        _ => throw new ArgumentException($"Landmark {landmark} cannot be rendered", nameof(landmark))
    };

}
