using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation;

public sealed class EarthquakeSimulator : ISimulator
{

    private static readonly Length DistanceThreshold = Length.FromKilometers(10);

    private TimeSpan _remainingTime;

    public Earthquake Earthquake { get; }

    public bool Active => _remainingTime > TimeSpan.Zero;

    public EarthquakeSimulator(Earthquake earthquake)
    {
        Earthquake = earthquake;
        _remainingTime = earthquake.Duration;
    }

    public double GetStrengthMultiplier(Coordinates location)
    {
        var distance = PositionedRenderer.PixelSize * Math.Sqrt(Coordinates.DistanceSquared(location, Earthquake.Epicenter));
        return Math.Max(0, 1 - distance / DistanceThreshold);
    }

    public void Step(SimulatorSession session, TimeSpan time)
    {
        var casualtyChance = Earthquake.Strength.Newtons / 5000;
        foreach (var populationReducible in session.PopulationReducibles)
        {
            var strength = GetStrengthMultiplier(populationReducible.Location);
            if (strength != 0)
                populationReducible.KillPercentage(casualtyChance * Random.Shared.NextDouble() * strength);
        }

        _remainingTime -= time;
    }

}
