using System.Collections.Generic;

namespace YTS.Git.SubCommands
{
    /// <summary>
    /// 接口: Git - push 子命令
    /// </summary>
    public interface IGitPush
    {
        /// <summary>
        /// 执行命令: git push
        /// </summary>
        /// <returns>响应输出文本内容</returns>
        IList<string> OnCommand();

        /// <summary>
        /// 执行命令: git push {origin} {master}
        /// </summary>
        /// <param name="origin">远程仓库标识</param>
        /// <param name="branch">分支名称</param>
        /// <returns>响应输出文本内容</returns>
        IList<string> OnCommand(string origin, string branch);
    }
}
