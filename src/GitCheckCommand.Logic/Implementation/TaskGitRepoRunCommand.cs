using System.IO;
using System.Text;

using Newtonsoft.Json;

using YTS.Log;
using YTS.Git;

using GitCheckCommand.Logic.Models;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.Generic;

namespace GitCheckCommand.Logic.Implementation
{
    public class TaskGitRepoRunCommand : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        public TaskGitRepoRunCommand(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            this.commandOptions = commandOptions;
        }

        public string GetDescribe()
        {
            return "在仓库所在目录直接运行指定命令";
        }

        public TaskResponse OnExecute(GitRepository repository)
        {
            string command = (commandOptions.Command ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(command)) {
                return new TaskResponse()
                {
                    IsSuccess = true,
                    ErrorCode = ETaskResponseErrorCode.ParameterIsEmpty,
                    ErrorMessage = "未指定执行命令跳过执行",
                };
            }
            ProcessStartInfo info = new ProcessStartInfo(@"git", command)
            {
                UseShellExecute = false,
                WorkingDirectory = repository.Path.FullName,
                RedirectStandardOutput = true,
                StandardOutputEncoding = encoding,
            };
            using (Process process = Process.Start(info))
            {
                using (StreamReader sr = process.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        print.WriteLine(sr.ReadLine());
                    }
                    sr.Close();
                }
                process.WaitForExit();
                process.Close();
            }
            return new TaskResponse()
            {
                IsSuccess = true,
                ErrorCode = ETaskResponseErrorCode.None,
                ErrorMessage = string.Empty,
            };
        }
    }
}
