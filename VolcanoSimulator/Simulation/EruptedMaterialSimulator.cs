namespace VolcanoSimulator.Simulation;

public abstract class EruptedMaterialSimulator<T> : ISimulator where T : EruptedMaterialBase
{

    public required T Material { get; init; }

    public bool Active => !Material.HasDecayed;

    public abstract void Step(SimulatorSession session, TimeSpan time);

}
