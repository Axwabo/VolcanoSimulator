using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Volcano : NamedLandmark
{

    private static Density InitialCloudDensity(VolcanicExplosivityIndex index) => Density.FromKilogramsPerCubicMeter(1 - 1d / index.Index);

    private readonly List<EruptedMaterialBase> _eruptedMaterial = [];

    public IReadOnlyCollection<EruptedMaterialBase> EruptedMaterial => _eruptedMaterial.AsReadOnly();

    public required VolcanicExplosivityIndex ExplosivityIndex { get; init; }

    public Earthquake Erupt()
    {
        if (ExplosivityIndex.Index == 0)
            return new Earthquake
            {
                Duration = TimeSpan.FromMinutes(1),
                Epicenter = Location,
                Strength = Force.FromKilonewtons(1)
            };
        var totalEjecta = ExplosivityIndex.EjectaVolume;
        var lavaVolume = totalEjecta * 0.01d;
        _eruptedMaterial.Add(new AshCloud
        {
            Location = Location,
            Mass = InitialCloudDensity(ExplosivityIndex) * (totalEjecta - lavaVolume)
        });
        _eruptedMaterial.Add(new Lava
        {
            Location = Location,
            InitialTemperature = Temperature.FromDegreesCelsius(Random.Shared.Next(
                (int) Lava.MinInitialTemperature.DegreesCelsius,
                (int) Lava.MaxInitialTemperature.DegreesCelsius
            )),
            FlowAngle = Angle.FromDegrees(Random.Shared.NextDouble() * 360),
            Volume = lavaVolume
        });
        return new Earthquake
        {
            Duration = TimeSpan.FromMinutes(Random.Shared.Next(ExplosivityIndex.Index * 2, ExplosivityIndex.Index * 10)),
            Epicenter = Location,
            Strength = Force.FromNewtons((ExplosivityIndex.Index + 1) * 100)
        };
    }

    public void ClearDecayedMaterial() => _eruptedMaterial.RemoveAll(e => e.HasDecayed);

}
