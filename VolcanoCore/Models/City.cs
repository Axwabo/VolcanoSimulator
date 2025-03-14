﻿namespace VolcanoCore.Models;

public sealed class City : NamedLandmark, IEvacuationLocation, IPopulationReducible
{

    public int AccommodatedPeople { get; private set; }

    public int Capacity => int.MaxValue;

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
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
