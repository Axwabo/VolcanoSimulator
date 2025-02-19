using VolcanoSimulator;
using VolcanoSimulator.Rendering;
using VolcanoSimulator.Rendering.Extensions;
using VolcanoSimulator.Simulation;

Console.CursorVisible = false;
if (OperatingSystem.IsWindows() && !VirtualTerminalProcessing.EnableOutput())
{
    LavaColor.IsAnsiSupported = false;
    Console.WriteLine("Failed to enable virtual output. Lava color will always be red.");
    Console.WriteLine("Enter to continue...");
    Console.ReadLine();
}

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
