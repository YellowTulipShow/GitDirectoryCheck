using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class TaskGitRepoOpenShell : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        public TaskGitRepoOpenShell(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            this.commandOptions = commandOptions;
        }

        public string GetDescribe()
        {
            return "打开仓库所在目录的 Shell 窗口";
        }

        public TaskResponse OnExecute(GitRepository repository)
        {
            print.WriteLine($"任务: 打开Shell, 仓库: {repository.Path.FullName}");
            return new TaskResponse()
            {
                IsSuccess = true,
            };
        }
    }
}
