using UnitsNet;
using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class AshCloudRenderer : EruptedMaterialRenderer
{

    private static readonly Density MaxDisplayedDensity = Density.FromKilogramsPerLiter(1);

    private static readonly char[] Densities = ['@', 'G', '0', 'O', 'o', ':', '.'];

    private static double EaseInCubic(double x) => x * x * x;

    private static char GetDensityChar(AshCloud cloud)
    {
        var max = MaxDisplayedDensity.As(AshCloud.SafeDensity.Unit);
        var ratio = Math.Min(max, cloud.CurrentDensity.As(AshCloud.SafeDensity.Unit)) / max;
        var densityChar = Densities[Math.Min(Densities.Length - 1, (int) Math.Round(EaseInCubic(-Math.Log2(ratio) / Math.Log2(max)) * Densities.Length))];
        return densityChar;
    }

    public AshCloudRenderer(AshCloud material) : base(material)
    {
    }

    public override void Draw(in ViewportRect viewport)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        var cloud = (AshCloud) Positioned;
        Draw(viewport, cloud, GetDensityChar(cloud));
        Console.ResetColor();
    }

    public override void Clear(in ViewportRect viewport) => Draw(viewport, (AshCloud) Positioned, ' ');

    private static void Draw(ViewportRect viewport, AshCloud cloud, char densityChar)
    {
        var (originX, originY) = viewport.Transform(cloud.Location);
        var origin = new Coordinates(originY, originX);
        var sizePixels = (int) Math.Ceiling(cloud.Radius / PixelSize);
        var minY = Math.Max(0, origin.Latitude - sizePixels);
        var maxY = Math.Min(viewport.Size.Latitude, origin.Latitude + sizePixels);
        for (var y = minY; y < maxY; y++)
        {
            var minX = Math.Max(0, origin.Longitude - sizePixels);
            var maxX = Math.Min(viewport.Size.Longitude, origin.Longitude + sizePixels);
            Render.Cursor = new Coordinates(y, minX);
            for (var x = minX; x < maxX; x++)
                if (Coordinates.DistanceSquared(origin, new Coordinates(y, x)) <= sizePixels * sizePixels)
                    Console.Write(densityChar);
                else if (x < maxX - 1)
                    Console.CursorLeft++;
        }
    }

}
