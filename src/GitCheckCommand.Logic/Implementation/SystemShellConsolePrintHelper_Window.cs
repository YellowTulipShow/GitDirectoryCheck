using System;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// Window 系统类型 Shell 控制台打印输出实现类
    /// </summary>
    public class SystemShellConsolePrintHelper_Window : IPrint, IPrintColor
    {
        private int lineCount = 0;

        /// <summary>
        /// 实例化 - Window 系统类型 Shell 控制台打印输出实现类
        /// </summary>
        public SystemShellConsolePrintHelper_Window() { }

        /// <inheritdoc/>
        public int GetLineCount()
        {
            return lineCount;
        }

        /// <inheritdoc/>
        public void Write(string content)
        {
            Console.Write(content);
            if (content.Contains("\n"))
            {
                lineCount++;
            }
        }

        /// <inheritdoc/>
        public void WriteLine(string content)
        {
            Console.WriteLine(content);
            lineCount++;
        }

        /// <inheritdoc/>
        public void Write(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            SetConsoleColor(textColor, backgroundColor);
            this.Write(content);
            Console.ResetColor();
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            SetConsoleColor(textColor, backgroundColor);
            this.WriteLine(content);
            Console.ResetColor();
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
                EPrintColor.Green => ConsoleColor.Green,
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Window 控制台颜色, 无法解析: {printColor}"),
            };
        }
    }
}
