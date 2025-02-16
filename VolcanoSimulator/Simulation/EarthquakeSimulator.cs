using UnitsNet;
using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation;

public sealed class EarthquakeSimulator : ISimulator
{

    private static readonly Length DistanceThreshold = Length.FromKilometers(10);

    private TimeSpan _remainingTime;

    public required Earthquake Earthquake { get; init; }

    public bool Active => _remainingTime > TimeSpan.Zero;

    public void Step(SimulatorSession session, TimeSpan time)
    {
        var casualtyChance = Earthquake.Strength.Newtons / 5000;
        foreach (var city in session.Landmarks.OfType<City>())
        {
            var distance = PositionedRenderer.PixelSize * Math.Sqrt(Coordinates.DistanceSquared(city.Location, Earthquake.Epicenter));
            if (distance <= DistanceThreshold)
                city.Kill((int) (city.AccommodatedPeople * casualtyChance * distance / DistanceThreshold));
        }

        _remainingTime -= time;
    }

}
