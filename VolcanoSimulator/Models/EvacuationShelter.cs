namespace VolcanoSimulator.Models;

public sealed class EvacuationShelter : IEvacuationLocation
{

    public required Coordinates Location { get; init; }

    public required int ShelterCapacity { get; set; }

    public int People { get; private set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        People += people;
    }

    public void Move(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        if (people > People)
            throw new ArgumentOutOfRangeException(nameof(people), "Cannot move more people than the amount already in the shelter.");
        People -= people;
    }

}
