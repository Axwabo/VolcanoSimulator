namespace VolcanoSimulator.Models;

public interface IEvacuationLocation
{

    int ShelterCapacity => int.MaxValue;

    void Shelter(int people);

}
