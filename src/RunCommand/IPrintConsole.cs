namespace RunCommand
{
    /// <summary>
    /// 打印输出控制台控制台
    /// </summary>
    public interface IPrintConsole
    {
        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="content">消息内容</param>
        void Write(string content);

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="content">消息内容</param>
        void WriteIntervalLine(string content);
    }
}
