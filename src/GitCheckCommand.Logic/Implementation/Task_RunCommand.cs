using System.IO;
using System.Text;
using System.Diagnostics;

using YTS.Log;

using GitCheckCommand.Logic.Models;
using System.Text.RegularExpressions;
using System;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 任务实现类: 运行自定义命令
    /// </summary>
    public class Task_RunCommand : ITask
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly IPrintColor print;
        private readonly CommandOptions commandOptions;

        /// <summary>
        /// 实例化 - 任务实现类: 运行自定义命令
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        /// <param name="print">输出打印接口</param>
        /// <param name="commandOptions">命令选项配置</param>
        public Task_RunCommand(ILog log, Encoding encoding, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.print = print;
            this.commandOptions = commandOptions;
        }

        /// <inheritdoc/>
        public string GetDescribe()
        {
            return "在仓库所在目录直接运行指定命令";
        }

        /// <inheritdoc/>
        public TaskResponse OnExecute(GitRepository repository)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["taskDescribe"] = GetDescribe();
            string command = (commandOptions.Command ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(command))
            {
                return new TaskResponse() { Code = ETaskResponseCode.None };
            }
            logArgs["command"] = command;
            Regex commandRegex = new Regex(@"^([a-z]+)\s+([^\n]+)$",
                RegexOptions.ECMAScript | RegexOptions.IgnoreCase);
            logArgs["commandRegex"] = commandRegex.ToString();
            try
            {
                Match match = commandRegex.Match(command);
                if (!match.Success)
                {
                    return new TaskResponse()
                    {
                        Code = ETaskResponseCode.ParameterIsEmpty,
                        IsSuccess = false,
                        ErrorMessage = $"命令不符合规范, 命令: ({command}), 规范正则: /{commandRegex}/i",
                    };
                }
                string fileName = match.Groups[1].Value;
                logArgs["fileName"] = fileName;
                string arguments = match.Groups[2].Value;
                logArgs["arguments"] = arguments;
                ProcessStartInfo info = new ProcessStartInfo(fileName, arguments)
                {
                    UseShellExecute = false,
                    WorkingDirectory = repository.Path.FullName,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = encoding,
                };
                print.WriteLine("执行命令内容响应内容:");
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
                print.WriteSpaceLine();
                return new TaskResponse()
                {
                    IsSuccess = true,
                    Code = ETaskResponseCode.End,
                    ErrorMessage = string.Empty,
                };
            }
            catch (Exception ex)
            {
                log.Error($"运行任务出错!", ex, logArgs);
                return new TaskResponse()
                {
                    IsSuccess = false,
                    Code = ETaskResponseCode.Exception,
                    ErrorMessage = $"运行任务出错: {ex.Message}",
                };
            }
        }
    }
}
