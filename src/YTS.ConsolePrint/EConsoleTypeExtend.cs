using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using YTS.ConsolePrint.Implementation;

namespace YTS.ConsolePrint
{
    /// <summary>
    /// 静态扩展: 枚举: 系统类型
    /// </summary>
    public static class EConsoleTypeExtend
    {
        /// <summary>
        /// 根据系统配置或控制台标题, 获取默认配置的控制台类型
        /// </summary>
        /// <returns>控制台类型</returns>
        public static EConsoleType GetDefalutConsoleType()
        {
            string title = Console.Title.ToLower().Trim();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (title.Contains(@"invisible cygwin console"))
                {
                    return EConsoleType.WindowGitBash;
                }
                else if (title.Contains(@"powershell"))
                {
                    return EConsoleType.PowerShell;
                }
                return EConsoleType.CMD;
            }
            return EConsoleType.Bash;
        }

        /// <summary>
        /// 控制台类型转为打印接口实例
        /// </summary>
        /// <param name="consoleType">控制台类型</param>
        /// <returns>打印接口</returns>
        public static IPrintColor ToIPrintColor(this EConsoleType consoleType)
        {
            switch (consoleType)
            {
                case EConsoleType.CMD:
                    return new WindowSystemConsole_CMD();
                case EConsoleType.PowerShell:
                    return new WindowSystemConsole_PowerShell();
                case EConsoleType.Bash:
                    return new LinuxSystemConsole_Bash();
                case EConsoleType.WindowGitBash:
                    return new WindowSystemConsole_GitBash();
                default:
                    throw new ArgumentOutOfRangeException(nameof(consoleType), $"转为打印输出颜色接口, 无法解析: {consoleType}");
            }
        }
    }
}
