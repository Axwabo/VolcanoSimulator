using UnitsNet;
using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation;

public sealed class LavaSimulator : EruptedMaterialSimulator<Lava>
{

    private readonly List<City> _cities = [];

    public static Speed FlowRate { get; } = Speed.FromKilometersPerHour(8); // TODO: interpolate using height

    public static TemperatureChangeRate CoolingRate { get; } = TemperatureChangeRate.FromDegreesCelsiusPerHour(25);

    public override void Step(SimulatorSession session, TimeSpan time)
    {
        if (Material.CanFlow)
            Material.Flow(FlowRate * time);
        Material.Cool(CoolingRate * time);
        if (!Material.HasDecayed)
            ClaimLives(session);
    }

    private void ClaimLives(SimulatorSession session)
    {
        using var handler = new UniformCasualtyHandler(1, _cities, session);
        var (startX, startY) = Material.Location;
        var endX = startX + (int) (Material.Length / PositionedRenderer.PixelSize);
        var endY = startY + (int) (Material.Width / PositionedRenderer.PixelSize);
        if (startX > endX)
            (startX, endX) = (endX, startX);
        if (startY > endY)
            (startY, endY) = (endY, startY);
        for (var y = startY; y <= endY; y++)
        for (var x = startX; x <= endX; x++)
            handler.Process(new Coordinates(y, x));
    }

}
