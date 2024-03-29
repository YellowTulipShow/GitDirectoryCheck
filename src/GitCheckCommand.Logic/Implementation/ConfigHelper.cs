﻿using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using YTS.Log;
using YTS.ConsolePrint;

using GitCheckCommand.Logic.Models;
using System.Reflection;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 配置处理类
    /// </summary>
    public class ConfigHelper
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// 实例化 - 配置处理类
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        /// <param name="print">打印输出接口</param>
        public ConfigHelper(ILog log, Encoding encoding, IPrintColor print)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="configFilePath">配置文件路径指定</param>
        /// <param name="consoleType">控制台类型</param>
        /// <returns>配置内容</returns>
        public Configs ReadConfigs(string configFilePath, EConsoleType consoleType)
        {
            FileInfo file = new FileInfo(configFilePath);
            Configs defaultConfigs = GetDefaultConfigs(consoleType);
            if (!file.Exists)
            {
                file.Create().Close();
                string json = JsonConvert.SerializeObject(defaultConfigs, jsonSerializerSettings);
                File.WriteAllText(file.FullName, json, this.encoding);
                print.Write($"配置文件不存在, 自动创建默认项: ");
                print.WriteLine(file.FullName, EPrintColor.Red);
                return defaultConfigs;
            }
            string content = File.ReadAllText(file.FullName, this.encoding);
            Configs config = JsonConvert.DeserializeObject<Configs>(content);
            print.Write($"获取配置文件成功: ");
            print.WriteLine(file.FullName, EPrintColor.Green);
            if (config.Version != defaultConfigs.Version)
            {
                string oldVersion = config.Version;
                string newVersion = defaultConfigs.Version;
                config = Merge(defaultConfigs, config);
                config.Version = defaultConfigs.Version;
                string json = JsonConvert.SerializeObject(config, jsonSerializerSettings);
                File.WriteAllText(file.FullName, json, this.encoding);
                print.Write($"版本号过期: ");
                print.WriteLine($"[{oldVersion}] => [{newVersion}], ", EPrintColor.Red);
                print.Write($"更改配置文件!");
            }
            return config;
        }
        private static T Merge<T>(T obj1, T obj2)
        {
            JObject json_obj1 = JObject.FromObject(obj1);
            JObject json_obj2 = JObject.FromObject(obj2);
            json_obj1.Merge(json_obj2);
            return json_obj1.ToObject<T>();
        }

        private Configs GetDefaultConfigs(EConsoleType consoleType)
        {
            string version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return new Configs()
            {
                Version = version,
                IsOpenShell = false,
                OpenShellGitBashExePath = ToOpenShellGitBashExePath(consoleType),
                IgnoresRegexs = new string[] { },
                Roots = new ConfigRoot[] {
                    new ConfigRoot()
                    {
                        Path = ToConfigRootDefaultPath(consoleType),
                    },
                },
            };
        }

        private static string ToConfigRootDefaultPath(EConsoleType consoleType)
        {
            const string window = @"C:\Work";
            const string linux = @"/var/work";
            return consoleType switch
            {
                EConsoleType.CMD => window,
                EConsoleType.PowerShell => window,
                EConsoleType.Bash => linux,
                EConsoleType.WindowGitBash => window,
                _ => throw new ArgumentOutOfRangeException(nameof(consoleType), $"转为配置默认项根目录地址, 无法解析: {consoleType}"),
            };
        }

        private static string ToOpenShellGitBashExePath(EConsoleType consoleType)
        {
            const string window = @"C:\Program Files\Git\git-bash.exe";
            const string linux = @"/bin/git/git-bash";
            return consoleType switch
            {
                EConsoleType.CMD => window,
                EConsoleType.PowerShell => window,
                EConsoleType.Bash => linux,
                EConsoleType.WindowGitBash => window,
                _ => throw new ArgumentOutOfRangeException(nameof(consoleType), $"转为配置默认项根目录地址, 无法解析: {consoleType}"),
            };
        }
    }
}
