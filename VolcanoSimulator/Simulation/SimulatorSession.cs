using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public sealed class SimulatorSession
{

    public List<LandmarkBase> Landmarks { get; } = [];

    public HashSet<EruptedMaterialBase> AllEruptedMaterials { get; } = [];

    public void RefreshEruptedMaterial()
    {
        foreach (var volcano in Landmarks.OfType<Volcano>())
        {
            volcano.ClearDecayedMaterial();
            AllEruptedMaterials.UnionWith(volcano.EruptedMaterial);
        }

        AllEruptedMaterials.RemoveWhere(e => e.HasDecayed);
    }

}
