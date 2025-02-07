namespace VolcanoSimulator.Models;

public sealed class City : IEvacuationLocation
{

    public required string Name { get; set; }

    public required Coordinates Location { get; init; }

    public int CitizenCount { get; set; }

    public void Shelter(int people)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(people);
        CitizenCount += people;
    }

}
