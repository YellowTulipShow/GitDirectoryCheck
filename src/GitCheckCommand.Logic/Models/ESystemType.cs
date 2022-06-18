using System;

using GitCheckCommand.Logic.Implementation;

namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 系统类型
    /// </summary>
    public enum ESystemType
    {
        /// <summary>
        /// Microsoft Windows 系统类型: cmd / powershell
        /// </summary>
        Window,

        /// <summary>
        /// Linux 系统类型: bash
        /// </summary>
        Linux,

        /// <summary>
        /// Microsoft Windows 系统系统下使用的命令行: Mintty Git Bash
        /// </summary>
        WindowGitBash,
    }

    /// <summary>
    /// 静态扩展: 枚举: 系统类型
    /// </summary>
    public static class ESystemTypeExtend
    {
        /// <summary>
        /// 系统类型转为打印接口实例
        /// </summary>
        /// <param name="systemType">系统类型</param>
        /// <returns>打印接口</returns>
        public static IPrintColor ToIPrintColor(this ESystemType systemType)
        {
            return systemType switch
            {
                ESystemType.Window => new SystemShellConsolePrintHelper_Window(),
                ESystemType.Linux => new SystemShellConsolePrintHelper_Linux(),
                ESystemType.WindowGitBash => new SystemShellConsolePrintHelper_Linux(),
                _ => throw new ArgumentOutOfRangeException(nameof(systemType), $"转为打印输出颜色接口, 无法解析: {systemType}"),
            };
        }

        /// <summary>
        /// 转为配置根目录项默认路径
        /// </summary>
        /// <param name="systemType">系统路径</param>
        /// <returns>默认路径</returns>
        public static string ToConfigRootDefaultPath(this ESystemType systemType)
        {
            const string window = @"C:\Work";
            const string linux = @"/var/work";
            return systemType switch
            {
                ESystemType.Window => window,
                ESystemType.Linux => linux,
                ESystemType.WindowGitBash => window,
                _ => throw new ArgumentOutOfRangeException(nameof(systemType), $"转为配置默认项根目录地址, 无法解析: {systemType}"),
            };
        }
    }
}
