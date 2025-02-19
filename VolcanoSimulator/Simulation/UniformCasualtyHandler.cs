namespace VolcanoSimulator.Simulation;

public readonly ref struct UniformCasualtyHandler : IDisposable
{

    private readonly double _rate;
    private readonly List<IPopulationReducible> _populationCache;

    public UniformCasualtyHandler(double rate, List<IPopulationReducible> populationCache, SimulatorSession session)
    {
        CasualtyExtensions.ValidateRate(rate);
        _rate = rate;
        _populationCache = populationCache;
        _populationCache.AddRange(session.Landmarks.OfType<City>());
    }

    public void Process(in Coordinates location)
    {
        for (var i = 0; i < _populationCache.Count; i++)
        {
            var handler = _populationCache[i];
            if (handler.Location != location)
                continue;
            handler.KillPercentage(_rate);
            _populationCache.RemoveAt(i--);
        }
    }

    public void Dispose() => _populationCache.Clear();

}
