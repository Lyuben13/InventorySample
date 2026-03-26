using System.Runtime.InteropServices;

namespace InventoryManagement.Helpers;

/// <summary>
/// Помощни методи за настройка на изгледа на конзолата.
/// </summary>
public static class ConsoleAppearanceHelper
{
    // Import на Windows API функции за работа с конзолни шрифтове
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern bool SetCurrentConsoleFontEx(IntPtr hConsoleOutput, bool MaximumWindow, ref CONSOLE_FONT_INFOEX ConsoleFontInfoEx);

    [StructLayout(LayoutKind.Sequential)]
    public struct CONSOLE_FONT_INFOEX
    {
        public int cbSize;
        public int nFont;
        public short FontWidth;
        public short FontHeight;
        public int FontFamily;
        public int FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FontFace;
    }

    /// <summary>
    /// Опитва да настрои персонализиран шрифт за конзолата.
    /// </summary>
    public static void TrySetCustomFont()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            try
            {
                var hConsole = GetStdHandle(-11); // STD_OUTPUT_HANDLE
                var fontInfo = new CONSOLE_FONT_INFOEX
                {
                    cbSize = Marshal.SizeOf(typeof(CONSOLE_FONT_INFOEX)),
                    FontFace = "3403-font",
                    FontHeight = 16,
                    FontWidth = 8
                };

                // Първо опитваме с името на шрифта
                if (!SetCurrentConsoleFontEx(hConsole, false, ref fontInfo))
                {
                    // Ако не успее, опитваме с консола шрифт
                    fontInfo.FontFace = "Consolas";
                    SetCurrentConsoleFontEx(hConsole, false, ref fontInfo);
                }
            }
            catch
            {
                // Ако има грешка, продължаваме със стандартния шрифт
            }
        }
    }
}
