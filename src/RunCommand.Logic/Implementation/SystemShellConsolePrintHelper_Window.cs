using System;

using RunCommand.Logic.Models;

namespace RunCommand.Logic.Implementation
{
    /// <summary>
    /// Window ϵͳ���� Shell ����̨��ӡ���ʵ����
    /// </summary>
    public class SystemShellConsolePrintHelper_Window : IPrint, IPrintColor
    {
        /// <inheritdoc/>
        public void Write(string content)
        {
            Console.Write(content);
        }

        /// <inheritdoc/>
        public void WriteLine(string content)
        {
            Console.WriteLine(content);
        }

        /// <inheritdoc/>
        public void Write(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            ConsoleColor console_textColor = ToConsoleColor(textColor);
            ConsoleColor console_backgroundColor = ToConsoleColor(backgroundColor);
            Console.ForegroundColor = console_textColor;
            Console.BackgroundColor = console_backgroundColor;
            Console.Write(content);
            Console.ResetColor();
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            ConsoleColor console_textColor = ToConsoleColor(textColor);
            ConsoleColor console_backgroundColor = ToConsoleColor(backgroundColor);
            Console.ForegroundColor = console_textColor;
            Console.BackgroundColor = console_backgroundColor;
            Console.WriteLine(content);
            Console.ResetColor();
        }

        private static ConsoleColor ToConsoleColor(EPrintColor printColor)
        {
            return printColor switch
            {
                EPrintColor.Black => ConsoleColor.Black,
                EPrintColor.White => ConsoleColor.White,
                EPrintColor.Yellow => ConsoleColor.Yellow,
                EPrintColor.Red => ConsoleColor.Red,
                EPrintColor.Blue => ConsoleColor.Blue,
                EPrintColor.Purple => ConsoleColor.Magenta,
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"תΪ Window ����̨��ɫ, �޷�����: {printColor}"),
            };
        }
    }
}
