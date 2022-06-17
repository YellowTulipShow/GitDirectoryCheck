using YTS.Git.SubCommands;

namespace YTS.Git
{
    /// <summary>
    /// 接口: Git 操作工具
    /// </summary>
    public interface IGit
    {
        /// <summary>
        /// git init : 初始化创建 Git 存储库
        /// </summary>
        /// <returns>执行实现对象</returns>
        IGitInit Init();

        /// <summary>
        /// git add : 添加到暂存库子命令
        /// </summary>
        /// <returns>执行实现对象</returns>
        IGitAdd Add();

        /// <summary>
        /// git status : 查看存储库状态信息子命令
        /// </summary>
        /// <returns>执行实现对象</returns>
        IGitStatus Status();

        /// <summary>
        /// git commit : 提交到存储库子命令
        /// </summary>
        /// <returns>执行实现对象</returns>
        IGitCommit Commit();

        /// <summary>
        /// git pull : 拉取远程仓库数据子命令
        /// </summary>
        /// <returns>执行实现对象</returns>
        IGitPull Pull();

        /// <summary>
        /// git push : 推送本地到远程仓库子命令
        /// </summary>
        /// <returns>执行实现对象</returns>
        IGitPush Push();
    }
}
