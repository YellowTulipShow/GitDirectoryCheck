using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

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
            print.WriteLine($"任务: 执行检查状态, 仓库: {repository.Path.FullName}");
            return new TaskResponse()
            {
                IsSuccess = true,
            };
        }
    }
}
