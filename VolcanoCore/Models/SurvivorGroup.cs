namespace VolcanoCore.Models;

public sealed class SurvivorGroup : IPositioned
{

    public required Coordinates Location { get; set; }

    public required int People { get; init; }

    public required IEvacuationLocation Target { get; init; }

}
