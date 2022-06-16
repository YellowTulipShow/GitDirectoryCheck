using System;

using RunCommand.Logic.Models;

namespace RunCommand.Logic.Implementation
{
    /// <summary>
    /// Window 系统类型 Shell 控制台打印输出实现类
    /// </summary>
    public class SystemShellConsolePrintHelper_Window : IPrint, IPrintColor
    {
        private bool beforeIsIntervalLine;
        private string interval_line;
        public SystemShellConsolePrintHelper_Window()
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
            SetConsoleColor(textColor, backgroundColor);
            Console.Write(content);
            Console.ResetColor();
            this.beforeIsIntervalLine = false;
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            if (!this.beforeIsIntervalLine)
                Console.Write(interval_line);

            SetConsoleColor(textColor, backgroundColor);
            Console.Write(content);
            Console.ResetColor();

            Console.Write(interval_line);
            this.beforeIsIntervalLine = true;
        }
        private static void SetConsoleColor(EPrintColor textColor, EPrintColor backgroundColor)
        {
            var console_textColor = ToConsoleColor(textColor);
            if (console_textColor != null)
            {
                Console.ForegroundColor = (ConsoleColor)console_textColor;
            }
            var console_backgroundColor = ToConsoleColor(backgroundColor);
            if (console_backgroundColor != null)
            {
                Console.BackgroundColor = (ConsoleColor)console_backgroundColor;
            }
        }
        private static ConsoleColor? ToConsoleColor(EPrintColor printColor)
        {
            return printColor switch
            {
                EPrintColor.None => null,
                EPrintColor.Black => ConsoleColor.Black,
                EPrintColor.White => ConsoleColor.White,
                EPrintColor.Yellow => ConsoleColor.Yellow,
                EPrintColor.Red => ConsoleColor.Red,
                EPrintColor.Blue => ConsoleColor.Blue,
                EPrintColor.Purple => ConsoleColor.Magenta,
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Window 控制台颜色, 无法解析: {printColor}"),
            };
        }
    }
}
