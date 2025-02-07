using VolcanoSimulator.Models;
using VolcanoSimulator.Rendering;
using VolcanoSimulator.Simulation;

var session = new SimulatorSession
{
    Landmarks =
    {
        new City
        {
            Location = new Coordinates(5, 10),
            Name = "Pompeii"
        }
    }
};
var renderer = new SimulatorRenderer(session);
renderer.RedrawAll();

while (true)
{
    var key = Console.ReadKey();
    switch (key.Key)
    {
        case ConsoleKey.Escape:
            return;
        case ConsoleKey.UpArrow:
            renderer.Move(new Coordinates(1, 0));
            break;
    }
}
