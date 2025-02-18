using VolcanoSimulator.Rendering;
using VolcanoSimulator.Simulation;

Console.CursorVisible = false;
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
    if (!renderer.Input.Process(Console.ReadKey(true)))
        break;
Console.CursorVisible = true;
