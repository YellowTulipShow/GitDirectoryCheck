using System;
using System.IO;
using System.Text;

using YTS.Log;

using RunCommand.Logic;
using RunCommand.Logic.Implementation;

namespace RunCommand
{
    class Program
    {
        static int Main(string[] args)
        {
            Encoding encoding = Encoding.UTF8;
            try
            {
                var logFile = ILogExtend.GetLogFilePath("Program");
                ILog log = new FilePrintLog(logFile, encoding).Connect(new ConsolePrintLog());
                IMain im = new MainHelpr(log, encoding);
                CommandArgsParser commandArgsParser = new CommandArgsParser(log, im);
                return commandArgsParser.OnParser(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"程序出错: {ex.Message}");
                Console.WriteLine($"堆栈信息: {ex.StackTrace ?? string.Empty}");
                return 1;
            }
        }

        private static void Test_Main1()
        {

            Console.WriteLine("Hello World!");

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
