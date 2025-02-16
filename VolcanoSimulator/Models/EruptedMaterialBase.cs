namespace VolcanoSimulator.Models;

public abstract class EruptedMaterialBase : IPositioned
{

    public required Coordinates Location { get; init; }

    public abstract bool HasDecayed { get; }

}
