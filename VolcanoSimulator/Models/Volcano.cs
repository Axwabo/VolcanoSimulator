using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Volcano : NamedLandmark
{

    private static Density InitialCloudDensity(VolcanicExplosivityIndex index) => Density.FromKilogramsPerCubicMeter(1 - 1d / index.Index);

    private readonly List<EruptedMaterialBase> _eruptedMaterial = [];

    public IReadOnlyCollection<EruptedMaterialBase> EruptedMaterial => _eruptedMaterial.AsReadOnly();

    public required VolcanicExplosivityIndex ExplosivityIndex { get; init; }

    public void Erupt()
    {
        if (ExplosivityIndex.Index == 0)
            return;
        _eruptedMaterial.Add(new AshCloud
        {
            Location = Location,
            Mass = InitialCloudDensity(ExplosivityIndex) * ExplosivityIndex.EjectaVolume
        });
        _eruptedMaterial.Add(new Lava
        {
            Location = Location,
            InitialTemperature = Temperature.FromDegreesCelsius(Random.Shared.Next(
                (int) Lava.MinInitialTemperature.DegreesCelsius,
                (int) Lava.MaxInitialTemperature.DegreesCelsius
            ))
        });
    }

    public void ClearDecayedMaterial() => _eruptedMaterial.RemoveAll(e => e.HasDecayed);

}
