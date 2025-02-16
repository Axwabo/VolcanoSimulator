namespace VolcanoSimulator.Models;

public abstract class LandmarkBase : IPositioned
{

    public required Coordinates Location { get; init; }

}
