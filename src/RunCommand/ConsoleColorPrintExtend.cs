using System;

namespace RunCommand
{
    public static class ConsoleColorPrintExtend
    {
        public static void Write(this ConsoleColor color, string str)
        {
            ConsoleColor selfcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = selfcolor;
        }
        public static void WriteLine(this ConsoleColor color, string str)
        {
            ConsoleColor selfcolor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ForegroundColor = selfcolor;
        }
    }
}
