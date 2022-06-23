﻿using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;
using YTS.ConsolePrint;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 配置处理类
    /// </summary>
    public class ConfigHelper
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        /// <summary>
        /// 实例化 - 配置处理类
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        public ConfigHelper(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
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
            if (!file.Exists)
            {
                file.Create().Close();
                Configs defaultConfigs = GetDefaultConfigs(consoleType);
                string json = JsonConvert.SerializeObject(defaultConfigs, jsonSerializerSettings);
                File.WriteAllText(file.FullName, json, this.encoding);
                log.Error($"配置文件不存在, 自动创建默认项: {file.FullName}");
                return defaultConfigs;
            }
            string content = File.ReadAllText(file.FullName, this.encoding);
            var config = JsonConvert.DeserializeObject<Configs>(content);
            log.Info($"获取配置文件成功: {file.FullName}");
            return config;
        }

        private Configs GetDefaultConfigs(EConsoleType consoleType)
        {
            return new Configs()
            {
                IsOpenShell = false,
                IgnoresRegexs = new string[] { },
                Roots = new ConfigRoot[] {
                    new ConfigRoot()
                    {
                        Path = ToConfigRootDefaultPath(consoleType),
                        IgnoresRegexs = new string[] {
                            @"YTS.Test$",
                            @"YTS.Learn$",
                        },
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
    }
}
