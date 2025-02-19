using VolcanoSimulator.Rendering.Renderers;

namespace VolcanoSimulator.Simulation;

public sealed class SurvivorGroupSimulator : ISimulator
{

    private static readonly Speed AmbulanceSpeed = Speed.FromKilometersPerHour(60);

    private static readonly Coordinates[] Offsets =
    [
        new(1, 0),
        new(0, 1),
        new(-1, 0),
        new(0, -1)
    ];

    private bool _reachedTarget;

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

        if (_reachedTarget)
        {
            ShelterPeople();
            return;
        }

        var current = Group.Location;
        var target = Group.Target;
        var targetLocation = target.Location;
        var distance = Math.Sqrt(Coordinates.DistanceSquared(current, targetLocation)) * PositionedRenderer.PixelSize;
        var timeToTarget = distance / AmbulanceSpeed;
        if (timeToTarget <= time)
        {
            _reachedTarget = true;
            Group.Location = target.Location;
            ShelterPeople();
            return;
        }

        var direction = Math.Atan2(targetLocation.Latitude - current.Latitude, targetLocation.Longitude - current.Longitude);
        var step = AmbulanceSpeed * time;
        var stepX = step * Math.Cos(direction);
        var stepY = step * Math.Sin(direction);
        Group.Location += new Coordinates((int) (stepY / PositionedRenderer.PixelSize), (int) (stepX / PositionedRenderer.PixelSize));
    }

    private void ShelterPeople()
    {
        var target = Group.Target;
        var add = Math.Min(target.Capacity - target.AccommodatedPeople, Group.AccommodatedPeople);
        if (add > 0)
        {
            target.Shelter(add);
            Group.Remove(add);
        }

        Active = Group.AccommodatedPeople != 0;
        if (Active)
            Group.Location = target.Location + Offsets[Random.Shared.Next(Offsets.Length)];
    }

}
