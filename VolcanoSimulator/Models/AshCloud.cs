using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class AshCloud
{

    public static readonly Density SafeDensity = Density.FromMilligramsPerCubicMeter(4);

    public required Coordinates Origin { get; init; }

    public required Mass Mass { get; init; }

    public Length Radius { get; private set; } = Length.FromMeters(1 / Math.Cbrt(Math.PI));

    public bool Decayed => CurrentDensity <= SafeDensity;

    public Volume Volume => Math.PI * Radius * Radius * Radius;

    public Density CurrentDensity => Mass / Volume;

    public void Grow(Length amount)
    {
        if (amount.Meters <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Growth amount must be positive");
        Radius += amount;
    }

}
