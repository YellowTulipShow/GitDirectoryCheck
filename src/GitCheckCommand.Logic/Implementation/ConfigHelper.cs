using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class ConfigHelper
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public ConfigHelper(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
            jsonSerializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            };
        }

        public Configs ReadConfigs(string configFilePath, ESystemType systemType)
        {
            FileInfo file = new FileInfo(configFilePath);
            if (!file.Exists)
            {
                file.Create().Close();
                Configs defaultConfigs = GetDefaultConfigs(systemType);
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

        private Configs GetDefaultConfigs(ESystemType systemType)
        {
            return new Configs()
            {
                IsOpenShell = false,
                IgnoresRegexs = new string[] { },
                Roots = new ConfigRoot[] {
                    new ConfigRoot()
                    {
                        Path = systemType.ToConfigRootDefaultPath(),
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
