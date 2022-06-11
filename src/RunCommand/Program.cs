using System;

namespace RunCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConsoleColor.Red.WriteColorLine("Hello World!");

            String nl = Environment.NewLine;
            String[] colorNames = Enum.GetNames(typeof(ConsoleColor));

            Console.WriteLine("{0}All the foreground colors on a constant black background.", nl);
            Console.WriteLine("  (Black on black is not readable.){0}", nl);

            for (int x = 0; x < colorNames.Length; x++)
            {
                Console.Write("{0,2}: ", x);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorNames[x]);
                Console.Write("This is foreground color {0}.", colorNames[x]);
                Console.ResetColor();
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("/x01");
            Console.Write("/u0001");
            Console.Write("/001");
            Console.Write("/x10");
            Console.Write("/u0010");
            Console.Write("/020");

            Console.WriteLine();
            Console.Write("{0,-50}", "Class1.TestMethod1");
            Console.Write("{0,-2}", "/x10");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Pass");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("{0,-50}", "Class1.TestMethod2");
            Console.Write("{0,-2}", "/x10");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed");

            Console.ReadLine();
        }
    }
}
