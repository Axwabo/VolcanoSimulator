namespace VolcanoCore.Models;

public interface IEvacuationLocation : IPositioned
{

    int AccommodatedPeople { get; }

    int Capacity => int.MaxValue;

    void Shelter(int people);

    void Remove(int people);

}
