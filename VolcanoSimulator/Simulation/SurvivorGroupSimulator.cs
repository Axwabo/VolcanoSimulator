using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation;

public sealed class SurvivorGroupSimulator : ISimulator
{

    private static readonly Speed AmbulanceSpeed = Speed.FromKilometersPerHour(60);

    public SurvivorGroup Group { get; }

    public bool Active { get; private set; } = true;

    public SurvivorGroupSimulator(SurvivorGroup group) => Group = group;

    public void Step(SimulatorSession session, TimeSpan time)
    {
        var current = Group.Location;
        var target = Group.Target.Location;
        var distance = Math.Sqrt(Coordinates.DistanceSquared(current, target)) * PositionedRenderer.PixelSize;
        var timeToTarget = distance / AmbulanceSpeed;
        if (timeToTarget <= time)
        {
            Group.Target.Shelter(Group.People);
            Active = false;
            return;
        }

        var direction = Math.Atan2(target.Latitude - current.Latitude, target.Longitude - current.Longitude);
        var step = AmbulanceSpeed * time;
        var stepX = step * Math.Cos(direction);
        var stepY = step * Math.Sin(direction);
        Group.Location += new Coordinates((int) (stepY / PositionedRenderer.PixelSize), (int) (stepX / PositionedRenderer.PixelSize));
    }

}
