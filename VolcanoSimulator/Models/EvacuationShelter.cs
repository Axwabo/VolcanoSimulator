namespace VolcanoSimulator.Models;

public sealed class EvacuationShelter : LandmarkBase, IEvacuationLocation
{

    public required int ShelterCapacity { get; set; }

    public int AccommodatedPeople { get; private set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        AccommodatedPeople += people;
    }

    public void Move(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        if (people > AccommodatedPeople)
            throw new ArgumentOutOfRangeException(nameof(people), "Cannot move more people than the amount already in the shelter.");
        AccommodatedPeople -= people;
    }

}
