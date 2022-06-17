using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

using RunCommand.Logic.Models;

namespace RunCommand.Logic.Implementation
{
    public class ConfigHelper
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly JsonSerializerSettings jsonSerializerSettings;

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

        public Configs ReadConfigs(string configFilePath, ESystemType systemType)
        {
            FileInfo file = new FileInfo(configFilePath);
            if (!file.Exists)
            {
                file.Create().Close();
                Configs defaultConfigs = GetDefaultConfigs(systemType);
                string json = JsonConvert.SerializeObject(defaultConfigs, jsonSerializerSettings);
                File.WriteAllText(file.FullName, json, this.encoding);
                print.WriteLine($"配置文件不存在, 自动创建默认项: {file.FullName}", EPrintColor.Red);
                return defaultConfigs;
            }
            string content = File.ReadAllText(file.FullName, this.encoding);
            var config = JsonConvert.DeserializeObject<Configs>(content);
            print.Write($"获取配置文件成功: ");
            print.WriteLine($"{file.FullName}", EPrintColor.Blue);
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
                        Path = systemType == ESystemType.Window ?
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
