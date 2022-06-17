using System.Collections.Generic;

namespace YTS.Git.SubCommands
{
    /// <summary>
    /// 接口: Git - pull 子命令
    /// </summary>
    public interface IGitPull
    {
        /// <summary>
        /// 执行命令: git pull
        /// </summary>
        /// <param name="message">提交合并简称消息标题</param>
        /// <returns>响应输出文本内容</returns>
        IList<string> OnCommand(string message);

        /// <summary>
        /// 执行命令: git pull {origin} {master}
        /// </summary>
        /// <param name="origin">远程仓库标识</param>
        /// <param name="branch">分支名称</param>
        /// <param name="message">提交合并简称消息标题</param>
        /// <returns>响应输出文本内容</returns>
        IList<string> OnCommand(string origin, string branch, string message);
    }
}
