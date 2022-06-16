using System;
using System.IO;
using System.Text;
using System.Diagnostics;

using Newtonsoft.Json;

using YTS.Log;

using RunCommand.Logic.Models;

namespace RunCommand.Logic.Implementation
{
    public class MainHelpr : IMain
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public MainHelpr(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
            jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
        }

        private CommandOptions commandOptions;
        private IPrintColor print;
        private Configs configs;
        public void OnExecute(string configFilePath, CommandOptions commandOptions)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = commandOptions;
            this.commandOptions = commandOptions;

            print = commandOptions.SystemType.ToIPrintColor();
            logArgs["print.Type"] = print.GetType().Name;

            configs = ReadConfigs(configFilePath);
            logArgs["configs"] = configs;


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

        private Configs ReadConfigs(string configFilePath)
        {
            FileInfo file = new FileInfo(configFilePath);
            if (!file.Exists)
            {
                file.Create().Close();
                Configs defaultConfigs = GetDefaultConfigs();
                string json = JsonConvert.SerializeObject(defaultConfigs, jsonSerializerSettings);
                File.WriteAllText(file.FullName, json, this.encoding);
                return defaultConfigs;
            }
            string content = File.ReadAllText(file.FullName, this.encoding);
            return JsonConvert.DeserializeObject<Configs>(content);
        }

        private Configs GetDefaultConfigs()
        {
            return new Configs()
            {
                IsOpenShell = false,
                IgnoresRegexs = new string[] { },
                Roots = new ConfigRoot[] {
                    new ConfigRoot()
                    {
                        Path = this.commandOptions.SystemType == ESystemType.Window ?
                            @"D:\Work" :
                            @"/var/work",
                        IgnoresRegexs = new string[] {
                            @"YTS.Test$",
                            @"YTS.Learn$",
                        },
                    },
                },
            };
        }
    }
}
