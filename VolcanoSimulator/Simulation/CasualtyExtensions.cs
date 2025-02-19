namespace VolcanoSimulator.Simulation;

public static class CasualtyExtensions
{

    public static void ValidateRate(double rate)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rate);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(rate, 1);
    }

    public static void KillPercentage(this ICasualtyHandler handler, double rate)
    {
        ValidateRate(rate);
        var casualties = (int) (handler.AccommodatedPeople * rate);
        if (casualties != 0)
            handler.Remove(casualties);
    }

}
