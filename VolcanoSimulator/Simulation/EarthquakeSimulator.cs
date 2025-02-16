using UnitsNet;
using VolcanoSimulator.Models;
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

    public void Step(SimulatorSession session, TimeSpan time)
    {
        var casualtyChance = Earthquake.Strength.Newtons / 5000;
        foreach (var city in session.Landmarks.OfType<City>())
        {
            var distance = PositionedRenderer.PixelSize * Math.Sqrt(Coordinates.DistanceSquared(city.Location, Earthquake.Epicenter));
            if (distance > DistanceThreshold)
                continue;
            var kill = (int) (city.AccommodatedPeople * casualtyChance * (distance / DistanceThreshold) * Random.Shared.NextDouble());
            if (kill != 0)
                city.Kill(kill);
        }

        _remainingTime -= time;
    }

}
