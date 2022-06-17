using System;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// 静态扩展接口: 打印输出接口
    /// </summary>
    public static class IPrintExtend
    {
        /// <summary>
        /// 默认黑底, 写入内容
        /// </summary>
        /// <param name="printColor">打印输出接口含有颜色项</param>
        /// <param name="content">消息内容</param>
        /// <param name="textColor">文本内容颜色</param>
        public static void Write(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.Write(content, textColor, EPrintColor.None);
        }

        /// <summary>
        /// 默认黑底, 写入一行内容
        /// </summary>
        /// <param name="printColor">打印输出接口含有颜色项</param>
        /// <param name="content">消息内容</param>
        /// <param name="textColor">文本内容颜色</param>
        public static void WriteLine(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.WriteLine(content, textColor, EPrintColor.None);
        }

        private readonly static string interval_line = $"\n{"".PadLeft(80, '-')}\n";
        private static bool isBeforeWriteIntervalLine = false;
        /// <summary>
        /// 写入间隔行
        /// </summary>
        /// <param name="print">输出接口</param>
        public static void WriteIntervalLine(this IPrint print)
        {
            print.WriteLine(interval_line);
        }
    }
}
