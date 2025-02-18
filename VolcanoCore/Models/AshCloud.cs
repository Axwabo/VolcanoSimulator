namespace VolcanoCore.Models;

public sealed class AshCloud : EruptedMaterialBase
{

    public static Density SafeDensity { get; } = Density.FromMilligramsPerCubicMeter(4);

    public required Mass Mass { get; init; }

    public Length Radius { get; private set; } = Length.FromMeters(1 / Math.Cbrt(Math.PI));

    public override bool HasDecayed => CurrentDensity <= SafeDensity;

    public Volume Volume => Math.PI * Radius * Radius * Radius;

    public Density CurrentDensity => Mass / Volume;

    public void Grow(Length amount)
    {
        if (amount.Meters <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Growth amount must be positive");
        Radius += amount;
    }

}
