namespace VolcanoSimulator.Simulation;

public static class CasualtyExtensions
{

    public static void ValidateRate(double rate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rate);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rate, 1);
    }

    public static void KillPercentage(this IPopulationReducible population, double rate)
    {
        ValidateRate(rate);
        var casualties = (int) (population.AccommodatedPeople * rate);
        if (casualties != 0)
            population.Remove(casualties);
    }

}
