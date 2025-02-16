using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Lava : EruptedMaterialBase
{

    public static Temperature CoolTemperature { get; } = Temperature.FromDegreesCelsius(15);

    public static Temperature MinInitialTemperature { get; } = Temperature.FromDegreesCelsius(700);

    public static Temperature MaxInitialTemperature { get; } = Temperature.FromDegreesCelsius(2200);

    public required Temperature InitialTemperature
    {
        init
        {
            if (value < MinInitialTemperature || value > MaxInitialTemperature)
                throw new ArgumentOutOfRangeException(nameof(value), $"Initial temperature must be between {MinInitialTemperature} and {MaxInitialTemperature}");
            CurrentTemperature = value;
        }
    }

    public Temperature CurrentTemperature { get; private set; }

    public Length SideLength { get; private set; }

    public Area Area => SideLength * SideLength;

    public override bool HasDecayed => CurrentTemperature <= CoolTemperature;

    public void Grow(Length amount)
    {
        if (amount.Meters <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Growth amount must be positive");
        SideLength += amount;
    }

    public void Cool(TemperatureDelta amount)
    {
        if (amount.DegreesCelsius <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Cooling amount must be positive");
        CurrentTemperature -= amount;
        if (CurrentTemperature < CoolTemperature)
            CurrentTemperature = CoolTemperature;
    }

}
