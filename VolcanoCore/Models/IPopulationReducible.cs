namespace VolcanoCore.Models;

public interface IPopulationReducible : IPositioned
{

    int AccommodatedPeople { get; }

    void Remove(int people);

}
