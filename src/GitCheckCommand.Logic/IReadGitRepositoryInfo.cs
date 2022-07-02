using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// 接口: 读取 Git 存储库信息
    /// </summary>
    public interface IReadGitRepositoryInfo
    {
        /// <summary>
        /// 执行读取操作
        /// </summary>
        /// <param name="repository">存储库信息</param>
        /// <returns>读取完成后的存储库信息</returns>
        GitRepository OnExecute(GitRepository repository);
    }
}
