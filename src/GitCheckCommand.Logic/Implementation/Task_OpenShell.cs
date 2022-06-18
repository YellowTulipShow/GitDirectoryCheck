using System.Text;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 任务实现类: 打开Shell窗口
    /// </summary>
    public class Task_OpenShell : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        /// <summary>
        /// 实例化 - 任务实现类: 打开Shell窗口
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        /// <param name="print">输出打印接口</param>
        /// <param name="commandOptions">命令选项配置</param>
        public Task_OpenShell(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            this.commandOptions = commandOptions;
        }

        /// <inheritdoc/>
        public string GetDescribe()
        {
            return "打开仓库所在目录的 Shell 窗口";
        }

        /// <inheritdoc/>
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
