using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Volcano : NamedLandmark
{

    private static Temperature RandomLavaTemperature => Temperature.FromDegreesCelsius(Random.Shared.Next(
        (int) Lava.MinInitialTemperature.DegreesCelsius,
        (int) Lava.MaxInitialTemperature.DegreesCelsius
    ));

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
        var lavaVolume = totalEjecta * 0.01;
        _eruptedMaterial.Add(new AshCloud
        {
            Location = Location,
            Mass = InitialCloudDensity(ExplosivityIndex) * (totalEjecta - lavaVolume)
        });
        SpewLava(lavaVolume);
        return new Earthquake
        {
            Duration = TimeSpan.FromMinutes(Random.Shared.Next(ExplosivityIndex.Index * 2, ExplosivityIndex.Index * 10)),
            Epicenter = Location,
            Strength = Force.FromNewtons((ExplosivityIndex.Index + 1) * 100)
        };
    }

    private void SpewLava(Volume volume)
    {
        var count = Math.Clamp(Random.Shared.Next(3, 6), 0, ExplosivityIndex.Index);
        var volumePerInstance = volume / count;
        var volumeRandomness = volumePerInstance * 0.001;
        var angleDeltaPerInstance = 360d / count;
        var angle = Random.Shared.NextDouble() * 360;
        for (var i = 0; i < count; i++)
        {
            _eruptedMaterial.Add(new Lava
            {
                Location = Location,
                InitialTemperature = RandomLavaTemperature,
                FlowAngle = Angle.FromDegrees(angle),
                Volume = volumePerInstance + (Random.Shared.NextDouble() * 2 - 1) * volumeRandomness
            });
            angle += angleDeltaPerInstance + Random.Shared.Next(-10, 10);
        }
    }

    public void ClearDecayedMaterial() => _eruptedMaterial.RemoveAll(e => e.HasDecayed);

}
