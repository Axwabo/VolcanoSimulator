using UnitsNet;

namespace VolcanoSimulator.Models;

public readonly record struct VolcanicExplosivityIndex
{

    public const int MaxIndex = 8;

    public static Volume SmallestEjectaVolume { get; } = Volume.FromCubicKilometers(0.000001);

    public byte Index { get; }

    public double Multiplier => Math.Pow(10, Index);

    public Volume EjectaVolume => SmallestEjectaVolume * Multiplier;

    public VolcanicExplosivityIndex(byte index)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(index, MaxIndex);
        Index = index;
    }

}
