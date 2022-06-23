using System;

using YTS.ConsolePrint.Implementation;

namespace YTS.ConsolePrint
{
    /// <summary>
    /// 枚举: 系统类型
    /// </summary>
    public enum EConsoleType
    {
        /// <summary>
        /// Microsoft Windows 系统类型: cmd
        /// </summary>
        CMD,

        /// <summary>
        /// Microsoft Windows 系统类型: powershell
        /// </summary>
        PowerShell,

        /// <summary>
        /// Linux 系统类型: bash
        /// </summary>
        Bash,

        /// <summary>
        /// Microsoft Windows 系统系统下使用的命令行: Mintty Git Bash
        /// </summary>
        WindowGitBash,
    }
}
