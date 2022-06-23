using YTS.ConsolePrint;

namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 命令选项内容
    /// </summary>
    public struct CommandOptions
    {
        /// <summary>
        /// 当Git仓库'不干净'时, 是否需要自动打开命令窗口
        /// </summary>
        public bool? IsOpenShell { get; set; }

        /// <summary>
        /// 当Git仓库'不干净'时, 在其路径执行的命令内容
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 配置当前执行的控制台标识, 用于打印输出消息内容颜色使用
        /// </summary>
        public EConsoleType ConsoleType { get; set; }
    }
}
