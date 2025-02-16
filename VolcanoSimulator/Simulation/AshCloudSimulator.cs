using UnitsNet;
using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public sealed class AshCloudSimulator : EruptedMaterialSimulator<AshCloud>
{

    private static readonly Speed GrowthRate = Speed.FromMetersPerSecond(1);

    public override void Step(SimulatorSession session, TimeSpan time) => Material.Grow(GrowthRate * time);

}
