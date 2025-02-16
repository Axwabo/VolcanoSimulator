namespace VolcanoSimulator.Models;

public sealed class City : NamedLandmark, IEvacuationLocation
{

    public int AccommodatedPeople { get; private set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        AccommodatedPeople += people;
    }

    public void Kill(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        if (people > AccommodatedPeople)
            throw new InvalidOperationException("Cannot kill more people than there are accommodated");
        AccommodatedPeople -= people;
    }

}
