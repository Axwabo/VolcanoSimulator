using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Volcano : NamedLandmark
{

    private readonly List<EruptedMaterialBase> _eruptedMaterial = [];

    public IReadOnlyCollection<EruptedMaterialBase> EruptedMaterial => _eruptedMaterial.AsReadOnly();

    public required VolcanicExplosivityIndex ExplosivityIndex { get; init; }

    public void Erupt()
    {
        _eruptedMaterial.Add(new AshCloud
        {
            Origin = Location,
            Mass = Density.FromKilogramsPerCubicMeter(ExplosivityIndex.Index) * ExplosivityIndex.EjectaVolume
        });
        _eruptedMaterial.Add(new Lava
        {
            Origin = Location,
            InitialTemperature = Temperature.FromDegreesCelsius(Random.Shared.Next(
                (int) Lava.MinInitialTemperature.DegreesCelsius,
                (int) Lava.MaxInitialTemperature.DegreesCelsius
            ))
        });
    }

}
