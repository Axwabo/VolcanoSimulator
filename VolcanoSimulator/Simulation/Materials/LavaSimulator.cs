using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation.Materials;

public sealed class LavaSimulator : EruptedMaterialSimulator<Lava>
{

    // https://link.springer.com/article/10.1007/s004450050299#:~:text=At%20flow%20velocities%20of%201,proximal%20regions%20of%20the%20channel.
    // averages
    private static readonly Speed FlowRate = Speed.FromMetersPerSecond(1.5);
    private static readonly TemperatureDelta CoolingOverFirst2Km = TemperatureDelta.FromDegreesCelsius(13);
    private static readonly TemperatureChangeRate TransportCooling = TemperatureChangeRate.FromDegreesCelsiusPerHour(36);

    private static readonly Frequency MinCrystallizationRate = Frequency.FromCyclesPerHour(0.2);
    private static readonly Frequency MaxCrystallizationRate = Frequency.FromCyclesPerHour(0.5);

    private static Frequency RandomCrystallizationRate => new(
        (MaxCrystallizationRate - MinCrystallizationRate).As(MinCrystallizationRate.Unit) * Random.Shared.NextDouble() + MinCrystallizationRate.Value,
        MinCrystallizationRate.Unit
    );

    private readonly List<City> _cities = [];

    private double _previousTransportKm;

    private double TransportKm => Math.Sqrt(Math.Pow(Material.Width.Kilometers, 2) + Math.Pow(Material.Length.Kilometers, 2));

    public override void Step(SimulatorSession session, TimeSpan time)
    {
        if (Material.CanFlow)
            Material.Flow(FlowRate * time);
        Cool(time);
        if (!Material.HasDecayed)
            ClaimLives(session);
        _previousTransportKm = TransportKm;
    }

    private void Cool(TimeSpan time)
    {
        var transport = TransportKm;
        if (transport > _previousTransportKm)
        {
            var transportDelta = transport - _previousTransportKm;
            if (_previousTransportKm < 2)
                Material.Cool(transportDelta / 2 * CoolingOverFirst2Km);
            else
                Material.Cool(TransportCooling * time);
        }

        if (Material.CanFlow || Material.HasDecayed)
            return;
        var rate = RandomCrystallizationRate.PerSecond * time.TotalSeconds;
        Material.Cool((Material.InitialTemperature - Lava.CoolTemperature) * rate);
    }

    private void ClaimLives(SimulatorSession session)
    {
        using var handler = new UniformCasualtyHandler(1, _cities, session);
        var (startY, startX) = Material.Location;
        var endX = startX + (int) (Material.Width / PositionedRenderer.PixelSize);
        var endY = startY + (int) (Material.Length / PositionedRenderer.PixelSize);
        if (startX > endX)
            (startX, endX) = (endX, startX);
        if (startY > endY)
            (startY, endY) = (endY, startY);
        for (var y = startY; y <= endY; y++)
        for (var x = startX; x <= endX; x++)
            handler.Process(new Coordinates(y, x));
    }

}
