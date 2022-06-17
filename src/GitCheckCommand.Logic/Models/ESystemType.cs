using System;

using GitCheckCommand.Logic.Implementation;

namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 系统类型
    /// </summary>
    public enum ESystemType
    {
        Window,
        Linux,
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
                _ => throw new ArgumentOutOfRangeException(nameof(systemType), $"转为打印输出颜色接口, 无法解析: {systemType}"),
            };
        }
    }
}
