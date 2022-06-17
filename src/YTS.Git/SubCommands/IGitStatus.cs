using System.Collections.Generic;

namespace YTS.Git.SubCommands
{
    /// <summary>
    /// 接口: Git - status 子命令
    /// </summary>
    public interface IGitStatus
    {
        /// <summary>
        /// 执行命令: git status
        /// </summary>
        /// <returns>响应输出文本多行队列</returns>
        IList<string> OnCommand();
    }
}
