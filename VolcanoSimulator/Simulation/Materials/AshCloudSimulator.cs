using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation.Materials;

public sealed class AshCloudSimulator : EruptedMaterialSimulator<AshCloud>
{

    private static readonly Speed GrowthRate = Speed.FromMetersPerSecond(20);

    private static readonly Density FatalDensity = Density.FromGramsPerCubicMeter(10);

    private readonly List<IPopulationReducible> _populationCache = [];

    public override void Step(SimulatorSession session, TimeSpan time)
    {
        Material.Grow(GrowthRate * time);
        if (!Material.HasDecayed)
            ClaimLives(session);
    }

    private void ClaimLives(SimulatorSession session)
    {
        var rate = Math.Clamp((Material.CurrentDensity - AshCloud.SafeDensity) / FatalDensity, 0, 1);
        using var handler = new UniformCasualtyHandler(rate, _populationCache, session);
        var sizePixels = (int) Math.Ceiling(Material.Radius / PositionedRenderer.PixelSize);
        var origin = Material.Location;
        var (startX, startY) = (origin.Longitude - sizePixels, origin.Latitude - sizePixels);
        var (endX, endY) = (origin.Longitude + sizePixels, origin.Latitude + sizePixels);
        for (var y = startY; y <= endY; y++)
        for (var x = startX; x <= endX; x++)
            if (Coordinates.DistanceSquared(origin, new Coordinates(y, x)) <= sizePixels * sizePixels)
                handler.Process(new Coordinates(y, x));
    }

}
