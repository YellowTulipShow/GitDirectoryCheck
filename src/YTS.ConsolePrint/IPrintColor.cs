using System;
using System.Collections.Generic;
using System.Text;

namespace YTS.ConsolePrint
{
    /// <summary>
    /// 接口: 打印输出接口, 同时配置颜色设置
    /// </summary>
    public interface IPrintColor : IPrint
    {
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="textColor">文本内容颜色</param>
        /// <param name="backgroundColor">文本背景颜色</param>
        void Write(string content, EPrintColor textColor, EPrintColor backgroundColor);

        /// <summary>
        /// 写入一行内容
        /// </summary>
        /// <param name="content">消息内容</param>
        /// <param name="textColor">文本内容颜色</param>
        /// <param name="backgroundColor">文本背景颜色</param>
        void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor);
    }
}
