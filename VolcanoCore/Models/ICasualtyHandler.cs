namespace VolcanoCore.Models;

public interface ICasualtyHandler
{

    int AccommodatedPeople { get; }

    void Remove(int people);

}
