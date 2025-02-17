using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public readonly ref struct UniformCasualtyHandler : IDisposable
{

    private readonly double _rate;
    private readonly List<City> _cityCache;

    public UniformCasualtyHandler(double rate, List<City> cityCache, SimulatorSession session)
    {
        _rate = rate;
        _cityCache = cityCache;
        _cityCache.AddRange(session.Landmarks.OfType<City>());
    }

    public void Process(in Coordinates location)
    {
        for (var i = 0; i < _cityCache.Count; i++)
        {
            var city = _cityCache[i];
            if (city.Location != location)
                continue;
            city.KillPercentage(_rate);
            _cityCache.RemoveAt(i--);
        }
    }

    public void Dispose() => _cityCache.Clear();

}
