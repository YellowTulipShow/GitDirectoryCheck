using System;
using System.CommandLine;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RunCommand
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var mainHelper = GetMainHelper();
            return await CheckArgs(args, mainHelper);
        }

        private static MainHelper GetMainHelper()
        {
            var main = new MainHelper();
            return main;
        }
        private static async Task<int> CheckArgs(string[] args, MainHelper mainHelper)
        {
            var openbashOption = new Option<bool>(
                aliases: new string[] { "-o", "--openbash" },
                description: "是否需要自动打开命令窗口",
                getDefaultValue: () => false);
            var commandOption = new Option<string>(
                aliases: new string[] { "-c", "--command" },
                description: "如果仓库干净便执行的命令输入的命令");
            var rootCommand = new RootCommand("批量检查 Git 仓库状态");
            rootCommand.AddOption(openbashOption);
            rootCommand.AddOption(commandOption);
            rootCommand.SetHandler((isopenbash, command) =>
            {
                mainHelper.OnExecute(isopenbash, command);
            }, openbashOption, commandOption);
            return await rootCommand.InvokeAsync(args);
        }


        private static void Test_Main1()
        {

            Console.WriteLine("Hello World!");
            ConsoleColor.Red.WriteLine("Hello World!");

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
