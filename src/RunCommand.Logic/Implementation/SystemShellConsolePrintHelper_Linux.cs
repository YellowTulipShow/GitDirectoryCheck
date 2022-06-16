using System;

using RunCommand.Logic.Models;

namespace RunCommand.Logic.Implementation
{
    /// <summary>
    /// Linux ϵͳ���� Shell ����̨��ӡ���ʵ����
    /// </summary>
    public class SystemShellConsolePrintHelper_Linux : IPrint, IPrintColor
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
            string value_textColor = ToColorValue_Text(textColor);
            string value_backgroundColor = ToColorValue_BackgroundColor(backgroundColor);
            Console.Write($"\x1b[1;{value_backgroundColor};{value_textColor}m{content}\x1b[0m");
        }

        /// <inheritdoc/>
        public void WriteLine(string content, EPrintColor textColor, EPrintColor backgroundColor)
        {
            string value_textColor = ToColorValue_Text(textColor);
            string value_backgroundColor = ToColorValue_BackgroundColor(backgroundColor);
            Console.WriteLine($"\x1b[1;{value_backgroundColor};{value_textColor}m{content}\x1b[0m");
        }
        private static string ToColorValue_Text(EPrintColor printColor)
        {
            return printColor switch
            {
                EPrintColor.Black => "30",
                EPrintColor.White => "37",
                EPrintColor.Yellow => "33",
                EPrintColor.Red => "31",
                EPrintColor.Blue => "34",
                EPrintColor.Purple => "35",
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"תΪ Linux Shell �ı���ɫ, �޷�����: {printColor}"),
            };
        }
        private static string ToColorValue_BackgroundColor(EPrintColor printColor)
        {
            return printColor switch
            {
                EPrintColor.Black => "40",
                EPrintColor.White => "47",
                EPrintColor.Yellow => "43",
                EPrintColor.Red => "41",
                EPrintColor.Blue => "44",
                EPrintColor.Purple => "45",
                _ => throw new ArgumentOutOfRangeException(nameof(printColor), $"תΪ Linux Shell �ı���ɫ, �޷�����: {printColor}"),
            };
        }
    }
}
