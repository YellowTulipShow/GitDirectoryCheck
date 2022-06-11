using System;

namespace RunCommand
{
    public static class ConsoleColorPrintExtend
    {
        public static void WriteColorLine(this ConsoleColor color, string str)
        {
            ConsoleColor selfcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = selfcolor;
        }
    }
}
