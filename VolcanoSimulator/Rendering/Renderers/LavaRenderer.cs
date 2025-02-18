using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class LavaRenderer : EruptedMaterialRenderer
{

    public override MaterialLayer Layer => MaterialLayer.Lava;

    public LavaRenderer(Lava material) : base(material)
    {
    }

    public override void Draw(in ViewportRect viewport)
    {
        var lava = (Lava) Positioned;
        LavaColor.ColorBackground(lava.CurrentTemperature);
        Draw(viewport, lava);
        Console.ResetColor();
    }

    public override void Clear(in ViewportRect viewport) => Draw(viewport, (Lava) Positioned);

    private static void Draw(in ViewportRect viewport, Lava lava)
    {
        var (startX, startY) = viewport.Transform(lava.Location);
        var (endX, endY) = viewport.Transform(lava.Location + new Coordinates(
            (int) Math.Ceiling(lava.Length / PixelSize),
            (int) Math.Ceiling(lava.Width / PixelSize)
        ));
        if (startX > endX)
            (startX, endX) = (endX, startX);
        if (startY > endY)
            (startY, endY) = (endY, startY);
        var minX = Math.Max(0, startX);
        var minY = Math.Max(0, startY);
        var maxX = Math.Min(viewport.Size.Longitude, endX);
        var maxY = Math.Min(viewport.Size.Latitude, endY);
        for (var y = minY; y < maxY; y++)
        {
            Console.SetCursorPosition(minX, y);
            for (var x = minX; x < maxX; x++)
                Console.Write(' ');
        }
    }

}
