using GitCheckCommand.Logic.Models;

namespace GitCheckCommand
{
    /// <summary>
    /// 执行任务
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 获取任务描述
        /// </summary>
        /// <returns>描述文本</returns>
        string GetDescribe();

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="repository">存储库信息</param>
        /// <returns>任务响应信息</returns>
        TaskResponse OnExecute(GitRepository repository);
    }
}
