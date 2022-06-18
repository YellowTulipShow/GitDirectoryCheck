using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class Task_OpenShell : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        public Task_OpenShell(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
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
            bool isOpenShell = commandOptions.IsOpenShell ?? repository.IsOpenShell ?? false;
            if (!isOpenShell)
            {
                return new TaskResponse() { Code = ETaskResponseCode.None };
            }
            print.WriteLine($"执行打开Shell操作中, 请等待...", EPrintColor.Purple);
            return new TaskResponse()
            {
                IsSuccess = false,
                Code = ETaskResponseCode.NotFinished,
                ErrorMessage = "未实现打开Shell逻辑操作",
            };
        }
    }
}
