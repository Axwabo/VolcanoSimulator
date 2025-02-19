using Color = (int R, int G, int B);

namespace VolcanoSimulator.Rendering.Extensions;

public static class LavaColor
{

    private static readonly Color CoolColor = (0, 0, 0);
    private static readonly Color Red = (255, 0, 0);
    private static readonly Color Orange = (255, 128, 0);

    public static bool IsAnsiSupported { get; set; } = true;

    public static void ColorBackground(Temperature temperature)
    {
        Color color;
        if (temperature <= Lava.CoolTemperature)
            color = CoolColor;
        else if (temperature >= Lava.MaxInitialTemperature)
            color = Orange;
        else if (temperature < Lava.MinInitialTemperature)
            color = InterpolateColor(CoolColor, Red, Lava.CoolTemperature, Lava.MinInitialTemperature, temperature);
        else
            color = InterpolateColor(Red, Orange, Lava.MinInitialTemperature, Lava.MaxInitialTemperature, temperature);
        if (!IsAnsiSupported)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            return;
        }

        Console.Write("\e[48;2;");
        Console.Write(color.R);
        Console.Write(";");
        Console.Write(color.G);
        Console.Write(";");
        Console.Write(color.B);
        Console.Write("m");
    }

    private static Color InterpolateColor(Color min, Color max, Temperature minTemp, Temperature maxTemp, Temperature current)
    {
        var ratio = (current - minTemp) / (maxTemp - minTemp);
        return (
            (int) (min.R + ratio * (max.R - min.R)),
            (int) (min.G + ratio * (max.G - min.G)),
            (int) (min.B + ratio * (max.B - min.B))
        );
    }

}
