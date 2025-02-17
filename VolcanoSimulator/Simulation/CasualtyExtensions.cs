using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public static class CasualtyExtensions
{

    public static void KillPercentage(this City city, double rate)
    {
        var casualties = (int) (city.AccommodatedPeople * rate);
        if (casualties != 0)
            city.Kill(casualties);
    }

}
