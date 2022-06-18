using System;
using System.Collections.Generic;
using System.Text;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// 接口: 写入赋值 Git 存储库信息
    /// </summary>
    public interface IWriteGitRepositoryInfo
    {
        GitRepository OnExecute(GitRepository repository)
    }
}
