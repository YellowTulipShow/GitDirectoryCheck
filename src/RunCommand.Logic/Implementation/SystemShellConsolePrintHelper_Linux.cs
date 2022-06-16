using System;

using RunCommand.Logic.Models;

namespace RunCommand.Logic.Implementation
{
    /// <summary>
    /// Linux 系统类型 Shell 控制台打印输出实现类
    /// </summary>
    public class SystemShellConsolePrintHelper_Linux : IPrint, IPrintColor
    {
        private bool beforeIsIntervalLine;
        private string interval_line;
        public SystemShellConsolePrintHelper_Linux()
        {
            beforeIsIntervalLine = false;
            interval_line = $"\n{"".PadLeft(80, '-')}\n";
        }

        /// <inheritdoc/>
        public void Write(string content)
        {
            Console.Write($"{content}\n");
            this.beforeIsIntervalLine = false;
        }

        /// <inheritdoc/>
        public void WriteLine(string content)
        {
            if (!this.beforeIsIntervalLine)
                Console.Write(interval_line);
            Console.Write(content);
            Console.Write(interval_line);
            this.beforeIsIntervalLine = true;
        }

        /// <inheritdoc/>
        public void Write(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            string value_textColor = ToColorValue_Text(textColor);
            string value_backgroundColor = ToColorValue_BackgroundColor(backgroundColor);
            string mergeContnet = MergeContentAndColorFormat(content, value_textColor, value_backgroundColor);

            Console.Write($"{mergeContnet}\n");

            this.beforeIsIntervalLine = false;
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            if (!this.beforeIsIntervalLine)
                Console.Write(interval_line);

            string value_textColor = ToColorValue_Text(textColor);
            string value_backgroundColor = ToColorValue_BackgroundColor(backgroundColor);
            string mergeContnet = MergeContentAndColorFormat(content, value_textColor, value_backgroundColor);
            Console.Write(mergeContnet);

            Console.Write(interval_line);
            this.beforeIsIntervalLine = true;
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
            Console.WriteLine($"color: {color}");
            return $"\x1b[1;{color}m{content}\x1b[0m";
        }
    }
}
