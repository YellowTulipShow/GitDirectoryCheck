using System;

namespace YTS.ConsolePrint.Implementation
{
    /// <summary>
    /// 基础控制台打印输出实现类
    /// </summary>
    public class BasicConsole : IPrint
    {
        private int lineCount = 0;

        /// <summary>
        /// 实例化 - 基础控制台打印输出实现类
        /// </summary>
        public BasicConsole() { }

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
    }
}
