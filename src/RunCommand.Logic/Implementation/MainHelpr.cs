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
            print.WriteLine($"print.GetType().Name: {print.GetType().Name}");

            configs = ReadConfigs(configFilePath);
            logArgs["configs"] = configs;

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
                print.WriteLine($"配置文件不存在, 自动创建默认项: {file.FullName}", EPrintColor.Red);
                return defaultConfigs;
            }
            string content = File.ReadAllText(file.FullName, this.encoding);
            var config = JsonConvert.DeserializeObject<Configs>(content);
            print.Write($"获取配置文件成功:");
            print.WriteLine($"{file.FullName}", EPrintColor.Blue);
            return config;
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

        private ITask[] GetNeedExecuteITask()
        {
            return new ITask[]
            {
            };
        }
    }
}
