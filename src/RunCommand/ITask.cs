using System.Collections.Generic;

namespace RunCommand
{
    /// <summary>
    /// 执行任务
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// 执行任务内容
        /// </summary>
        /// <param name="repo">存储库</param>
        /// <returns>回传存储库信息</returns>
        IRepository OnExecute(IRepository repo);

        /// <summary>
        /// 输出打印结果
        /// </summary>
        /// <param name="repo">存储库</param>
        /// <returns>打印结果</returns>
        IList<string> PrintResult(IRepository repo);

        /// <summary>
        /// 是否将输出分割
        /// </summary>
        /// <param name="repo">存储库</param>
        /// <returns>结果</returns>
        bool IsWriteIntervalLine(IRepository repo);
    }
}
