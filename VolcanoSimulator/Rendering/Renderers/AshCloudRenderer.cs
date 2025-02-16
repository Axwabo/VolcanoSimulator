using UnitsNet;
using VolcanoSimulator.Models;

namespace VolcanoSimulator.Rendering.Renderers;

public sealed class AshCloudRenderer : EruptedMaterialRenderer
{

    private static readonly Density MaxDisplayedDensity = Density.FromKilogramsPerLiter(1);

    private static readonly char[] Densities = ['.', ':', 'o', 'O', '0', '@', '#'];

    public AshCloudRenderer(AshCloud material) : base(material)
    {
    }

    public override void Draw(in ViewportRect viewport)
    {
        var cloud = (AshCloud) Positioned;
        if (!viewport.TryTransform(cloud.Location, out var origin))
            return;
        var longitudePixels = (int) Math.Ceiling(cloud.Radius / PixelSize);
        var latitudePixels = (int) Math.Ceiling(cloud.Radius / PixelSize);
        var densityChar = Densities[Math.Min(Densities.Length - 1, (int) (cloud.CurrentDensity / MaxDisplayedDensity * Densities.Length))];
        Console.ForegroundColor = ConsoleColor.Gray;
        var minY = Math.Max(0, origin.Latitude - latitudePixels);
        var maxY = Math.Min(viewport.Size.Latitude, origin.Latitude + latitudePixels);
        for (var y = minY; y < maxY; y++)
        {
            Render.Cursor = new Coordinates(y, origin.Longitude - longitudePixels);
            var minX = Math.Max(0, origin.Longitude - longitudePixels);
            var maxX = Math.Min(viewport.Size.Longitude, origin.Longitude + longitudePixels);
            for (var x = minX; x < maxX; x++)
                Console.Write(densityChar);
        }

        Console.ResetColor();
    }

    public override void Clear(in ViewportRect viewport)
    {
        // TODO
    }

}
