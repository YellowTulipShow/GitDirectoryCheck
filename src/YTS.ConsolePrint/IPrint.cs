namespace YTS.ConsolePrint
{
    /// <summary>
    /// 接口: 打印输出接口
    /// </summary>
    public interface IPrint
    {
        /// <summary>
        /// 获取写入的行数
        /// </summary>
        /// <returns>行数</returns>
        int GetLineCount();

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="content">消息内容</param>
        void Write(string content);

        /// <summary>
        /// 写入一行内容
        /// </summary>
        /// <param name="content">消息内容</param>
        void WriteLine(string content);
    }
}
