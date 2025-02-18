using System.Diagnostics.CodeAnalysis;

namespace VolcanoSimulator.Simulation;

public static class EruptedMaterialSimulatingExtensions
{

    public static bool TryCreateSimulator(this EruptedMaterialBase material, [NotNullWhen(true)] out ISimulator? simulator)
    {
        simulator = material switch
        {
            AshCloud cloud => new AshCloudSimulator {Material = cloud},
            Lava lava => new LavaSimulator {Material = lava},
            _ => null
        };
        return simulator is not null;
    }

}
