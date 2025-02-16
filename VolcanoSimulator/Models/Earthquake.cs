using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Earthquake
{

    public required Coordinates Epicenter { get; init; }

    public required TimeSpan Duration { get; init; }

    public required Force Strength { get; init; }

}
