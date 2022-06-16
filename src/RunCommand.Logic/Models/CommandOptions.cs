namespace RunCommand.Logic.Models
{
    /// <summary>
    /// 命令选项内容
    /// </summary>
    public struct CommandOptions
    {
        /// <summary>
        /// 当Git仓库'不干净'时, 是否需要自动打开命令窗口
        /// </summary>
        public bool IsOpenShell { get; set; }

        /// <summary>
        /// 当Git仓库'不干净'时, 在其路径执行的命令内容
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 显示的执行当前执行的系统标识, 用于打印输出消息内容颜色使用
        /// </summary>
        public ESystemType SystemType { get; set; }
    }
}
