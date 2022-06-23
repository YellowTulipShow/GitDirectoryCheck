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
            switch (printColor)
            {
                case EPrintColor.None: return null;
                case EPrintColor.Black: return "30";
                case EPrintColor.White: return "37";
                case EPrintColor.Yellow: return "33";
                case EPrintColor.Red: return "31";
                case EPrintColor.Blue: return "34";
                case EPrintColor.Purple: return "35";
                case EPrintColor.Green: return "32";
                default: throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Linux Shell 文本颜色, 无法解析: {printColor}");
            }
        }
        private static string ToColorValue_BackgroundColor(EPrintColor printColor)
        {
            switch (printColor)
            {
                case EPrintColor.None: return null;
                case EPrintColor.Black: return "40";
                case EPrintColor.White: return "47";
                case EPrintColor.Yellow: return "43";
                case EPrintColor.Red: return "41";
                case EPrintColor.Blue: return "44";
                case EPrintColor.Purple: return "45";
                case EPrintColor.Green: return "42";
                default: throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Linux Shell 背景颜色, 无法解析: {printColor}");
            }
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
