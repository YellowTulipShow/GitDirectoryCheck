using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class TaskGitRepoRunCommand : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        private readonly GitHelper gitHelper;

        public TaskGitRepoRunCommand(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            this.commandOptions = commandOptions;
            gitHelper = new GitHelper(new Repository()
            {
                
            });
        }

        public string GetDescribe()
        {
            return "在仓库所在目录直接运行指定命令";
        }

        public TaskResponse OnExecute(GitRepository repository)
        {
            print.WriteLine($"任务: 执行自定义命令, 仓库: {repository.Path.FullName}");
            return new TaskResponse()
            {
                IsSuccess = true,
            };
        }
    }
}
