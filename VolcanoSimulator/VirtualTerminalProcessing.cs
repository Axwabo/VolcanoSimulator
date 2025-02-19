using System.Runtime.InteropServices;

namespace VolcanoSimulator;

public static class VirtualTerminalProcessing
{

    // ReSharper disable InconsistentNaming

    private const int STD_OUTPUT_HANDLE = -11;

    private const uint ENABLE_PROCESSED_OUTPUT = 0x0001;

    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

    [DllImport("kernel32")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    [DllImport("kernel32", SetLastError = true)]
    private static extern IntPtr GetStdHandle(int nStdHandle);

    public static bool IsSupported { get; private set; } = true;

    public static bool EnableOutput()
    {
        var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

        if (!GetConsoleMode(iStdOut, out var outConsoleMode))
            return false;

        outConsoleMode |= ENABLE_PROCESSED_OUTPUT | ENABLE_VIRTUAL_TERMINAL_PROCESSING;

        return SetConsoleMode(iStdOut, outConsoleMode);
    }

}
