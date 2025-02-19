namespace VolcanoCore.Models;

public sealed class EvacuationShelter : LandmarkBase, IEvacuationLocation
{

    public required int Capacity { get; init; }

    public int AccommodatedPeople { get; private set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        if (AccommodatedPeople + people > Capacity)
            throw new InvalidOperationException("Cannot shelter more people than the shelter's capacity");
        AccommodatedPeople += people;
    }

    public void Remove(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        if (people > AccommodatedPeople)
            throw new InvalidOperationException("Cannot remove more people than there are accommodated");
        AccommodatedPeople -= people;
    }

}
