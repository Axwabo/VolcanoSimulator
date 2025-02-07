using VolcanoSimulator.Models;
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
{
    var key = Console.ReadKey(true);
    switch (key.Key)
    {
        case ConsoleKey.Escape or ConsoleKey.X:
            return;
        case ConsoleKey.UpArrow:
            renderer.Move(new Coordinates(-1, 0));
            break;
        case ConsoleKey.DownArrow:
            renderer.Move(new Coordinates(1, 0));
            break;
        case ConsoleKey.LeftArrow:
            renderer.Move(new Coordinates(0, -1));
            break;
        case ConsoleKey.RightArrow:
            renderer.Move(new Coordinates(0, 1));
            break;
    }
}
