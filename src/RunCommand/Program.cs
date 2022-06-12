using System;
using System.IO;
using System.Text;

namespace RunCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_Main1();
            // Test_Main2(args);
        }

        private static int Test_Main2(string[] args)
        {
            // size of tab
            const int Val1 = 2;

            // working string
            const string Val_Text = "Geeks for Geeks";
            // check for the argument
            if (args.Length < 2)
            {
                Console.WriteLine(Val_Text);
                return 1;
            }

            try
            {
                // replacing space characters in a string with
                // a tab character
                using (var wrt1 = new StreamWriter(args[1]))
                {
                    using (var rdr1 = new StreamReader(args[0]))
                    {
                        Console.SetOut(wrt1);
                        Console.SetIn(rdr1);
                        string line;
                        while ((line = Console.ReadLine()) != null)
                        {
                            string newLine = line.Replace(("").PadRight(Val1, ' '), "\t");
                            Console.WriteLine(newLine);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                TextWriter errwrt = Console.Error;
                errwrt.WriteLine(e.Message);
                return 1;
            }

            // use of OpenStandardOutput() method
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;

            // set the output
            Console.SetOut(standardOutput);
            Console.WriteLine("OpenStandardOutput Example");
            return 0;
        }
        private static void Test_Main1()
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

            using (Stream outputSteam = Console.OpenStandardOutput())
            {
                outputSteam.Write(Encoding.UTF8.GetBytes("\x1b[32m he GGGGGGGGG \x1b[0m"));
            }

            Console.WriteLine("This is a \x1b[1;35m test \x1b[0m!");
            Console.WriteLine("This is a \x1b[1;32;43m test \x1b[0m!");
            Console.WriteLine("\x1b[1;33;44mThis is a test !\x1b[0m");

            Console.WriteLine("\x1b[32m he EEEEEEEE \x1b[0m");

            Console.WriteLine("\x1b[36mTEST\x1b[0m");
        }
    }
}
