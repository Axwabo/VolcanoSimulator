using System.Diagnostics.CodeAnalysis;
using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public static class EruptedMaterialSimulatingExtensions
{

    public static bool TryCreateSimulator(this EruptedMaterialBase material, [NotNullWhen(true)] out ISimulator? simulator)
    {
        simulator = material switch
        {
            AshCloud cloud => new AshCloudSimulator {Material = cloud},
            _ => null
        };
        return simulator is not null;
    }

}
