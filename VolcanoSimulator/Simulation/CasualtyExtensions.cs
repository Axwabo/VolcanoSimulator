using VolcanoSimulator.Models;

namespace VolcanoSimulator.Simulation;

public static class CasualtyExtensions
{

    public static void ClaimLives(this City city, double chance)
    {
        var casualties = (int) (city.AccommodatedPeople * chance * Random.Shared.NextDouble());
        if (casualties != 0)
            city.Kill(casualties);
    }

}
