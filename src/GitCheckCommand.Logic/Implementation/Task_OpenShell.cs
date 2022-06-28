using System;
using System.Text;

using YTS.Log;
using YTS.ConsolePrint;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 任务实现类: 打开Shell窗口
    /// </summary>
    public class Task_OpenShell : Task_RunCommand, ITask
    {
        private readonly Configs configs;

        /// <summary>
        /// 实例化 - 任务实现类: 打开Shell窗口
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="print">输出打印接口</param>
        /// <param name="commandOptions">命令选项配置</param>
        /// <param name="configs">配置文件项</param>
        public Task_OpenShell(ILog log, IPrintColor print, CommandOptions commandOptions, Configs configs)
            : base(log, print, commandOptions)
        {
            this.configs = configs;
        }

        /// <inheritdoc/>
        public override string GetDescribe()
        {
            return "打开仓库所在目录的 Shell 窗口";
        }

        /// <inheritdoc/>
        public override TaskResponse OnExecute(GitRepository repository)
        {
            bool isOpenShell = commandOptions.IsOpenShell ?? repository.IsOpenShell ?? false;
            if (!isOpenShell)
            {
                return new TaskResponse() { Code = ETaskResponseCode.None };
            }
            print.WriteLine($"执行打开Shell操作中, 请等待...", EPrintColor.Purple);

            string git_exe_path = configs.OpenShellGitBashExePath?.Trim();
            if (string.IsNullOrEmpty(git_exe_path))
                throw new Exception("Git Bash 程序地址未配置, 请检查!");

            string command;
            switch (commandOptions.ConsoleType)
            {
                case EConsoleType.CMD:
                case EConsoleType.PowerShell:
                case EConsoleType.WindowGitBash:
                    command = @$"powershell -Command ""Start-Process -FilePath '{git_exe_path}' -ArgumentList '--cd={repository.Path.FullName}'""";
                        break;
                case EConsoleType.Bash:
                    throw new Exception("未实现执行 Linux Bash 打开命令");
                default:
                    throw new ArgumentOutOfRangeException(nameof(commandOptions.ConsoleType), $"未实现打开Shell逻辑操作, 无法解析: {commandOptions.ConsoleType}");
            }
            print.WriteLine($"执行命令: {command}");
            return RunCommand(repository, command);
        }
    }
}
