using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// 接口: 写入赋值 Git 存储库信息
    /// </summary>
    public interface IWriteGitRepositoryInfo
    {
        /// <summary>
        /// 执行写入内容操作
        /// </summary>
        /// <param name="repository">存储库信息</param>
        /// <returns>写入完成后的存储库信息</returns>
        GitRepository OnExecute(GitRepository repository);
    }
}
