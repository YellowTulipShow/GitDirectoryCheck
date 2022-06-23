using System;
using System.Text.RegularExpressions;

namespace YTS.ConsolePrint
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
    }
}
