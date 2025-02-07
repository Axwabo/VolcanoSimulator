namespace VolcanoSimulator.Models;

public sealed class City : LandmarkBase, IEvacuationLocation
{

    public required string Name { get; set; }

    public int AccommodatedPeople { get; private set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        AccommodatedPeople += people;
    }

}
