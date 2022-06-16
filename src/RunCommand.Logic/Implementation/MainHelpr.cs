using System;
using System.Text;

using YTS.Log;

using RunCommand.Logic.Models;
using System.Diagnostics;

namespace RunCommand.Logic.Implementation
{
    public class MainHelpr : IMain
    {
        private readonly ILog log;
        private readonly Encoding encoding;

        public MainHelpr(ILog log)
        {
            this.log = log;
            encoding = Encoding.UTF8;
        }

        public void OnExecute(string configFilePath, CommandOptions commandOptions)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = commandOptions;

            //log.Error("配置文件不存在, 创建项!");

            IPrintColor[] printColors = new IPrintColor[]
            {
                new SystemShellConsolePrintHelper_Linux(),
                new SystemShellConsolePrintHelper_Window(),
            };
            foreach (var print in printColors)
            {
                Console.WriteLine($" ------------ ");
                Console.WriteLine($"Name: {print.GetType().FullName}");
                print.Write("普通文本内容1");
                print.Write("普通文本内容2");
                print.WriteLine("单行文本内容1");
                print.WriteLine("单行文本内容2");
                foreach (EPrintColor color in Enum.GetValues(typeof(EPrintColor)))
                {
                    print.WriteLine($"[黑底{color}字]", color, EPrintColor.Black);
                    print.WriteLine($"[白底{color}字]", color, EPrintColor.White);
                }
            }

            using Process cur = Process.GetCurrentProcess();
            //当前进程的id
            Console.WriteLine($"cur.Id: {cur.Id}");
            //获取关联的进程的终端服务会话标识符。
            Console.WriteLine($"cur.SessionId: {cur.SessionId}");
            //当前进程的名称
            Console.WriteLine($"cur.ProcessName: {cur.ProcessName}");
            //当前进程的启动时间
            Console.WriteLine($"cur.StartTime: {cur.StartTime}");
            //获取关联进程终止时指定的值,在退出事件中使用
            //Console.WriteLine(cur.ExitCode);
            //获取进程的当前机器名称
            Console.WriteLine($"cur.MachineName: {cur.MachineName}");
            //.代表本地
            //获取进程的主窗口标题。
            Console.WriteLine($"cur.MainWindowTitle: {cur.MainWindowTitle}");

            Console.WriteLine($"Console.Title: {Console.Title}");
        }

        private IPrintColor ToIPrintColor(ESystemType SystemType)
        {



        }
    }
}
