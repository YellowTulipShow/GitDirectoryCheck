using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 配置项
    /// </summary>
    public struct Configs
    {
        /// <summary>
        /// 当Git仓库'不干净'时, 是否需要自动打开命令窗口
        /// </summary>
        [JsonProperty(Order = 1)]
        public bool? IsOpenShell { get; set; }

        /// <summary>
        /// 打开 Git Bash 命令行程序路径地址配置
        /// </summary>
        [JsonProperty(Order = 2)]
        public string OpenShellGitBashExePath { get; set; }

        /// <summary>
        /// 查找 Git 仓库时进行屏蔽的路径正则匹配字符串队列
        /// </summary>
        [JsonProperty(Order = 3)]
        public string[] IgnoresRegexs { get; set; }

        /// <summary>
        /// 查找 Git 仓库从哪些目录开始
        /// </summary>
        [JsonProperty(Order = 4)]
        public ConfigRoot[] Roots { get; set; }
    }

    /// <summary>
    /// 配置项 - 根目录信息
    /// </summary>
    public struct ConfigRoot
    {
        /// <summary>
        /// 根目录路径
        /// </summary>
        [JsonProperty(Order = 0)]
        public string Path { get; set; }

        /// <summary>
        /// 当Git仓库'不干净'时, 是否需要自动打开命令窗口
        /// </summary>
        [JsonProperty(Order = 1)]
        public bool? IsOpenShell { get; set; }

        /// <summary>
        /// 查找 Git 仓库时进行屏蔽的路径正则匹配字符串队列
        /// </summary>
        [JsonProperty(Order = 2)]
        public string[] IgnoresRegexs { get; set; }
    }
}
