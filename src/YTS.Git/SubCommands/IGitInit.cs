using System.Collections.Generic;

namespace YTS.Git.SubCommands
{
    /// <summary>
    /// 接口: Git - init 子命令
    /// </summary>
    public interface IGitInit
    {
        /// <summary>
        /// 执行命令: git init
        /// </summary>
        void OnCommand();
    }
}
