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
        if (Group.AccommodatedPeople == 0)
        {
            Active = false;
            return;
        }

        var current = Group.Location;
        var target = Group.Target;
        var targetLocation = target.Location;
        var distance = Math.Sqrt(Coordinates.DistanceSquared(current, targetLocation)) * PositionedRenderer.PixelSize;
        var timeToTarget = distance / AmbulanceSpeed;
        if (timeToTarget <= time)
        {
            Group.Location = target.Location;
            var add = target.Capacity - Group.AccommodatedPeople;
            if (add < 0)
                return;
            target.Shelter(add);
            Group.Remove(add);
            Active = Group.AccommodatedPeople != 0;
            return;
        }

        var direction = Math.Atan2(targetLocation.Latitude - current.Latitude, targetLocation.Longitude - current.Longitude);
        var step = AmbulanceSpeed * time;
        var stepX = step * Math.Cos(direction);
        var stepY = step * Math.Sin(direction);
        Group.Location += new Coordinates((int) (stepY / PositionedRenderer.PixelSize), (int) (stepX / PositionedRenderer.PixelSize));
    }

}
