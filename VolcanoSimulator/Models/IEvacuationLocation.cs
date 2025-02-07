namespace VolcanoSimulator.Models;

public interface IEvacuationLocation
{

    int AccommodatedPeople { get; }

    int ShelterCapacity => int.MaxValue;

    void Shelter(int people);

}
