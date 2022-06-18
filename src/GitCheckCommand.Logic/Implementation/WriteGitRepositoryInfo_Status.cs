using System.Linq;
using System.Text;
using System.Collections.Generic;

using YTS.Log;

using YTS.Git.Command;
using YTS.Git.Implementation;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 写入存储库信息实现类: 状态信息
    /// </summary>
    public class WriteGitRepositoryInfo_Status : IWriteGitRepositoryInfo
    {
        private readonly ILog log;
        private readonly Encoding encoding;

        /// <summary>
        /// 实例化 - 写入存储库信息实现类: 状态信息
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        public WriteGitRepositoryInfo_Status(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
        }

        /// <inheritdoc/>
        public GitRepository OnExecute(GitRepository repository)
        {
            IGitStatus gitStatus = new GitStatusHelper(new YTS.Git.Models.Repository()
            {
                RootPath = repository.Path,
                OutputTextEncoding = encoding,
            });
            IList<string> msgs = gitStatus.OnCommand();
            (bool isClean, int noCleanMsgIndex) = CheckStatusMessageIsClean(msgs);
            repository.Status = new GitRepositoryStatus()
            {
                IsClean = isClean,
                StatusMsgs = msgs.ToArray(),
                NoCleanMsgIndex = noCleanMsgIndex,
            };
            return repository;
        }

        private (bool, int) CheckStatusMessageIsClean(IList<string> msgs)
        {
            string[] yes = new string[] {
                @"Changes not staged for commit:",
                @"Changes to be committed:",
                "(use \"git push\" to publish your local commits)",
                @"Untracked files:",
            };
            for (int i = 0; i < yes.Length; i++)
            {
                if (msgs.Contains(yes[i]))
                {
                    return (false, i);
                }
            }

            string[] no = new string[] {
                @"nothing to commit, working tree clean",
            };
            for (int i = 0; i < no.Length; i++)
            {
                if (msgs.Contains(no[i]))
                {
                    return (true, i);
                }
            }
            return (true, -1);
        }

    }
}
