using System;
using System.IO;
using System.Text;

using YTS.Log;

using GitCheckCommand.Logic;
using GitCheckCommand.Logic.Implementation;

namespace GitCheckCommand
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
    }
}
