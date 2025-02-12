namespace VolcanoSimulator.Models;

public sealed class City : NamedLandmark, IEvacuationLocation
{

    public int AccommodatedPeople { get; private set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        AccommodatedPeople += people;
    }

}
