namespace VolcanoSimulator.Models;

public interface IEvacuationLocation : IPositioned
{

    int AccommodatedPeople { get; }

    int ShelterCapacity => int.MaxValue;

    void Shelter(int people);

}
