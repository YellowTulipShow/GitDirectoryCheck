using System;

namespace YTS.ConsolePrint.Implementation
{
    /// <summary>
    /// Linux系统 - Bash类型实现
    /// </summary>
    public class LinuxSystemConsole_Bash : BasicConsole, IPrint, IPrintColor
    {
        /// <summary>
        /// 实例化 - Linux系统 - Bash类型实现
        /// </summary>
        public LinuxSystemConsole_Bash() : base() { }

        /// <inheritdoc/>
        public void Write(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            string value_textColor = ToColorValue_Text(textColor);
            string value_backgroundColor = ToColorValue_BackgroundColor(backgroundColor);
            string mergeContnet = MergeContentAndColorFormat(content, value_textColor, value_backgroundColor);
            base.Write($"{mergeContnet}");
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            string value_textColor = ToColorValue_Text(textColor);
            string value_backgroundColor = ToColorValue_BackgroundColor(backgroundColor);
            string mergeContnet = MergeContentAndColorFormat(content, value_textColor, value_backgroundColor);
            base.WriteLine(mergeContnet);
        }
        private static string ToColorValue_Text(EPrintColor printColor)
        {
            return printColor switch
            {
                EPrintColor.None => null,
                EPrintColor.Black => "30",
                EPrintColor.White => "37",
                EPrintColor.Yellow => "33",
                EPrintColor.Red => "31",
                EPrintColor.Blue => "34",
                EPrintColor.Purple => "35",
                EPrintColor.Green => "32",
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Linux Shell 文本颜色, 无法解析: {printColor}"),
            };
        }
        private static string ToColorValue_BackgroundColor(EPrintColor printColor)
        {
            return printColor switch
            {
                EPrintColor.None => null,
                EPrintColor.Black => "40",
                EPrintColor.White => "47",
                EPrintColor.Yellow => "43",
                EPrintColor.Red => "41",
                EPrintColor.Blue => "44",
                EPrintColor.Purple => "45",
                EPrintColor.Green => "42",
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Linux Shell 文本颜色, 无法解析: {printColor}"),
            };
        }
        private static string MergeContentAndColorFormat(string content, string value_textColor, string value_backgroundColor)
        {
            if (!string.IsNullOrEmpty(value_textColor) && !string.IsNullOrEmpty(value_backgroundColor))
            {
                return $"\x1b[1;{value_backgroundColor};{value_textColor}m{content}\x1b[0m";
            }
            else if (string.IsNullOrEmpty(value_textColor) && string.IsNullOrEmpty(value_backgroundColor))
            {
                return content;
            }
            string color = !string.IsNullOrEmpty(value_textColor) ?
                value_textColor : value_backgroundColor;
            return $"\x1b[1;{color}m{content}\x1b[0m";
        }
    }
}
