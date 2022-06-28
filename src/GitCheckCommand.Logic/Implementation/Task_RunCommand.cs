using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using YTS.Log;
using YTS.ConsolePrint;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 任务实现类: 运行自定义命令
    /// </summary>
    public class Task_RunCommand : ITask
    {
        /// <summary>
        /// 日志接口
        /// </summary>
        protected readonly ILog log;
        /// <summary>
        /// 文件编码
        /// </summary>
        protected readonly Encoding encoding;
        /// <summary>
        /// 打印输出接口
        /// </summary>
        protected readonly IPrintColor print;
        /// <summary>
        /// 命令参数
        /// </summary>
        protected readonly CommandOptions commandOptions;

        /// <summary>
        /// 实例化 - 任务实现类: 运行自定义命令
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="print">输出打印接口</param>
        /// <param name="commandOptions">命令选项配置</param>
        public Task_RunCommand(ILog log, IPrintColor print, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = Encoding.UTF8;
            this.print = print;
            this.commandOptions = commandOptions;
        }

        /// <inheritdoc/>
        public virtual string GetDescribe()
        {
            return "在仓库所在目录直接运行指定命令";
        }

        /// <inheritdoc/>
        public virtual TaskResponse OnExecute(GitRepository repository)
        {
            string command = (commandOptions.Command ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(command))
            {
                return new TaskResponse()
                {
                    Code = ETaskResponseCode.None,
                };
            }
            return RunCommand(repository, command);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="repository">存储库</param>
        /// <param name="command">命令字符串</param>
        /// <returns>执行结果</returns>
        protected TaskResponse RunCommand(GitRepository repository, string command)
        {
            var logArgs = log.CreateArgDictionary();
            string describe = GetDescribe();
            logArgs["taskDescribe"] = describe;
            logArgs["command"] = command;
            if (string.IsNullOrEmpty(command))
            {
                return new TaskResponse()
                {
                    Code = ETaskResponseCode.ParameterIsEmpty,
                    IsSuccess = false,
                    ErrorMessage = "执行命令内容为空",
                };
            }
            Regex commandRegex = new Regex(@"^([a-z_-]+)\s+([^\n]+)$",
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
                    Code = ETaskResponseCode.End,
                    IsSuccess = true,
                    ErrorMessage = string.Empty,
                };
            }
            catch (Exception ex)
            {
                string name = $"运行任务, {describe}出错";
                log.Error($"{name}!", ex, logArgs);
                return new TaskResponse()
                {
                    Code = ETaskResponseCode.Exception,
                    IsSuccess = false,
                    ErrorMessage = $"{name}: {ex.Message}",
                };
            }
        }
    }
}
