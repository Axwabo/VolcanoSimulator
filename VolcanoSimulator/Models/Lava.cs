using UnitsNet;

namespace VolcanoSimulator.Models;

public sealed class Lava : EruptedMaterialBase
{

    public static Length MinFlowingHeight { get; } = Length.FromCentimeters(1);

    public static Temperature CoolTemperature { get; } = Temperature.FromDegreesCelsius(15);

    public static Temperature MinInitialTemperature { get; } = Temperature.FromDegreesCelsius(700);

    public static Temperature MaxInitialTemperature { get; } = Temperature.FromDegreesCelsius(2200);

    private readonly Angle _flowAngle;

    private Length _height;

    public required Temperature InitialTemperature
    {
        init
        {
            if (value < MinInitialTemperature || value > MaxInitialTemperature)
                throw new ArgumentOutOfRangeException(nameof(value), $"Initial temperature must be between {MinInitialTemperature} and {MaxInitialTemperature}");
            CurrentTemperature = value;
        }
    }

    public required Angle FlowAngle
    {
        get => _flowAngle;
        init
        {
            _flowAngle = value;
            if (Math.Cos(value.Radians) < 0)
                Width *= -1;
            if (Math.Sin(value.Radians) < 0)
                Length *= -1;
        }
    }

    public required Volume Volume
    {
        get
        {
            var volume = Width * Length * _height;
            return volume.CubicMeters < 0 ? -volume : volume;
        }
        init => _height = CalculateHeight(value);
    }

    public Temperature CurrentTemperature { get; private set; }

    public Length Width { get; private set; } = Length.FromMeters(1);

    public Length Length { get; private set; } = Length.FromMeters(1);

    public bool CanFlow => !HasDecayed && _height > MinFlowingHeight;

    public override bool HasDecayed => CurrentTemperature <= CoolTemperature;

    private Length CalculateHeight(Volume volume)
    {
        var height = volume / Width / Length;
        return height.Meters < 0 ? -height : height;
    }

    public void Flow(Length amount)
    {
        if (!CanFlow)
            throw new InvalidOperationException("The lava can no longer flow");
        if (amount.Meters <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Flow length must be positive");
        var volume = Volume;
        Width += amount * Math.Cos(FlowAngle.Radians);
        Length += amount * Math.Sin(FlowAngle.Radians);
        _height = CalculateHeight(volume);
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
