using System.Collections.Generic;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand
{
    /// <summary>
    /// 执行任务
    /// </summary>
    public interface ITask
    {
        string GetDescribe();

        TaskResponse OnExecute(GitRepository repository);
    }
}
