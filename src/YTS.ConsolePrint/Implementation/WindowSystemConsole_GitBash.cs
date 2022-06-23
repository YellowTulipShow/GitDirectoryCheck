using System;

namespace YTS.ConsolePrint.Implementation
{
    /// <summary>
    /// Window系统 - Git Mintty Bash类型实现
    /// </summary>
    public class WindowSystemConsole_GitBash : LinuxSystemConsole_Bash, IPrint, IPrintColor
    {
        /// <summary>
        /// 实例化 - Window系统 - Git Mintty Bash类型实现
        /// </summary>
        public WindowSystemConsole_GitBash() : base() { }
    }
}
