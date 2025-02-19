namespace VolcanoCore.Models;

public sealed class SurvivorGroup : IPopulationReducible
{

    public required Coordinates Location { get; set; }

    public required int AccommodatedPeople { get; set; }

    public required IEvacuationLocation Target { get; set; }

    public void Remove(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        if (people > AccommodatedPeople)
            throw new ArgumentOutOfRangeException(nameof(people), "Cannot remove more people than there are in the group");
        AccommodatedPeople -= people;
    }

}
