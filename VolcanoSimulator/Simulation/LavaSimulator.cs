using UnitsNet;
using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public sealed class LavaSimulator : EruptedMaterialSimulator<Lava>
{

    public static Speed FlowRate { get; } = Speed.FromKilometersPerHour(8); // TODO: interpolate using height

    public static TemperatureChangeRate CoolingRate { get; } = TemperatureChangeRate.FromDegreesCelsiusPerHour(25);

    public override void Step(SimulatorSession session, TimeSpan time)
    {
        if (Material.CanFlow)
            Material.Flow(FlowRate * time);
        Material.Cool(CoolingRate * time);
    }

}
