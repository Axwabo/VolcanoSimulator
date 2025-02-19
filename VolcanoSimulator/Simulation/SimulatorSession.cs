using VolcanoSimulator.Simulation.Materials;

namespace VolcanoSimulator.Simulation;

public sealed class SimulatorSession
{

    public List<LandmarkBase> Landmarks { get; } = [];

    public HashSet<EruptedMaterialBase> AllEruptedMaterials { get; } = [];

    private readonly Dictionary<object, ISimulator> _simulators = [];

    private readonly List<object> _objectsToClear = [];

    public bool AnyActive => _simulators.Values.Any(e => e.Active);

    public IEnumerable<EarthquakeSimulator> EarthquakeSimulators => _simulators.Values.OfType<EarthquakeSimulator>();

    public IEnumerable<SurvivorGroup> SurvivorGroups => _simulators.Keys.OfType<SurvivorGroup>();

    public IEnumerable<IPopulationReducible> PopulationReducibles => _simulators.Keys.OfType<IPopulationReducible>()
        .Concat(Landmarks.OfType<IPopulationReducible>())
        .Distinct();

    public void RefreshEruptedMaterial()
    {
        foreach (var volcano in Landmarks.OfType<Volcano>())
        {
            volcano.ClearDecayedMaterial();
            foreach (var erupted in volcano.EruptedMaterial)
                if (AllEruptedMaterials.Add(erupted) && erupted.TryCreateSimulator(out var simulator))
                    _simulators[erupted] = simulator;
        }

        AllEruptedMaterials.RemoveWhere(e => e.HasDecayed);
    }

    public void RegisterEarthquake(Earthquake earthquake) => _simulators[earthquake] = new EarthquakeSimulator(earthquake);

    public void RegisterSurvivorGroup(SurvivorGroup group) => _simulators[group] = new SurvivorGroupSimulator(group);

    public void Step(TimeSpan time)
    {
        try
        {
            foreach (var (obj, simulator) in _simulators)
            {
                simulator.Step(this, time);
                if (!simulator.Active)
                    _objectsToClear.Add(obj);
            }

            foreach (var o in _objectsToClear)
                _simulators.Remove(o);
            RefreshEruptedMaterial();
        }
        finally
        {
            _objectsToClear.Clear();
        }
    }

}
