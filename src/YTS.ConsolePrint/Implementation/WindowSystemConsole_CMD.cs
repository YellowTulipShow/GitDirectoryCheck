using System;

namespace YTS.ConsolePrint.Implementation
{
    /// <summary>
    /// Window系统 - cmd类型实现
    /// </summary>
    public class WindowSystemConsole_CMD : BasicConsole, IPrint, IPrintColor
    {
        /// <summary>
        /// 实例化 - Window系统 - cmd类型实现
        /// </summary>
        public WindowSystemConsole_CMD() : base() { }

        /// <inheritdoc/>
        public void Write(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            SetConsoleColor(textColor, backgroundColor);
            base.Write(content);
            Console.ResetColor();
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            SetConsoleColor(textColor, backgroundColor);
            base.WriteLine(content);
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
            switch (printColor)
            {
                case EPrintColor.None: return null;
                case EPrintColor.Black: return ConsoleColor.Black;
                case EPrintColor.White: return ConsoleColor.White;
                case EPrintColor.Yellow: return ConsoleColor.Yellow;
                case EPrintColor.Red: return ConsoleColor.Red;
                case EPrintColor.Blue: return ConsoleColor.Blue;
                case EPrintColor.Purple: return ConsoleColor.Magenta;
                case EPrintColor.Green: return ConsoleColor.Green;
                default: throw new ArgumentOutOfRangeException(nameof(printColor), $"转为 Window 控制台颜色, 无法解析: {printColor}");
            }
        }
    }
}
