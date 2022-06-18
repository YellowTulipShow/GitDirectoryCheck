using System.Text;
using System.Collections.Generic;

using YTS.Log;

using YTS.Git.Command;
using YTS.Git.Implementation;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class TaskGitRepoCheckStatus : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        public TaskGitRepoCheckStatus(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            this.commandOptions = commandOptions;
        }

        public string GetDescribe()
        {
            return "检查仓库是否含有更改项";
        }

        public TaskResponse OnExecute(GitRepository repository)
        {
            IGitStatus gitStatus = new GitStatusHelper(new YTS.Git.Models.Repository()
            {
                RootPath = repository.Path,
                OutputTextEncoding = encoding,
            });
            IList<string> msgs = gitStatus.OnCommand();
            (bool isClean, int noCleanMsgIndex) = CheckStatusMessageIsClean(msgs);

            if (isClean)
            {
                return new TaskResponse()
                {
                    IsSuccess = true,
                    ErrorCode = ETaskResponseErrorCode.None,
                    ErrorMessage = string.Empty,
                };
            }
            print.WriteIntervalLine();
            print.WriteLine("当前仓库需要处理:\n");
            for (int i = 0; i < msgs.Count; i++)
            {
                string msg = msgs[i];
                if (noCleanMsgIndex == i)
                {
                    print.WriteLine(msg, EPrintColor.Purple);
                    continue;
                }
                print.WriteLine(msg);
            };
            return new TaskResponse()
            {
                IsSuccess = false,
                ErrorCode = ETaskResponseErrorCode.NotClean,
                ErrorMessage = "仓库需要处理",
            };
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
