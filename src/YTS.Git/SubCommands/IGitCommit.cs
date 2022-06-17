namespace YTS.Git.SubCommands
{
    /// <summary>
    /// 接口: Git - commit 子命令
    /// </summary>
    public interface IGitCommit
    {
        /// <summary>
        /// 执行命令: git commit -m "{message}"
        /// </summary>
        /// <param name="message">提交简称消息标题</param>
        void OnCommand(string message);
    }
}
