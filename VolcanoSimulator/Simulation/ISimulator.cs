namespace VolcanoSimulator.Simulation;

public interface ISimulator
{

    bool Active { get; }

    void Step(SimulatorSession session, TimeSpan time);

}
