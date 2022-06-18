using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class MainHelpr : IMain
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public MainHelpr(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
        }

        public void OnExecute(string configFilePath, CommandOptions commandOptions)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = commandOptions;

            IPrintColor print = commandOptions.SystemType.ToIPrintColor();
            string printTypeName = print.GetType().Name;
            logArgs["printTypeName"] = printTypeName;

            Configs configs = new ConfigHelper(log, encoding, print)
                .ReadConfigs(configFilePath, commandOptions.SystemType);
            GitRepository[] gitRepos = new FindGitRepositoryHelper(log)
                .OnExecute(configs);
            ITask[] tasks = GetNeedExecuteITask(print, commandOptions);

            var initGitRepositoryHelper = new InitGitRepositoryInfoHelper(log, encoding, commandOptions);
            for (int index_gitRepo = 0; index_gitRepo < gitRepos.Length; index_gitRepo++)
            {
                GitRepository gitRepo = gitRepos[index_gitRepo];
                gitRepo = initGitRepositoryHelper.OnExecute(gitRepo);
                logArgs["gitRepo.Path.FullName"] = gitRepo.Path.FullName;

                bool isSuccess = true;
                foreach (ITask task in tasks)
                {
                    string taskTypeName = task.GetType().Name;
                    logArgs["taskTypeName"] = taskTypeName;
                    string taskDesc = task.GetDescribe();
                    logArgs["taskDesc"] = taskDesc;
                    TaskResponse response = task.OnExecute(gitRepo);
                    if (!response.IsSuccess)
                    {
                        isSuccess = false;
                        print.WriteLine($"任务: [{taskDesc}]({taskTypeName}) 执行失败!");
                        print.WriteLine($"响应错误: [{response.ErrorCode}] {response.ErrorMessage}");
                        print.WriteIntervalLine();
                    }
                }

                print.WriteLine("项目路径:");
                string branchName = gitRepo.BranchName;
                if (branchName == "master")
                {
                    print.Write($"({branchName})");
                }
                else
                {
                    print.Write("(");
                    print.Write(branchName, EPrintColor.Red);
                    print.Write(")");
                }
                print.Write(" | ");
                string path = gitRepo.Path.FullName;
                if (commandOptions.SystemType == ESystemType.Window)
                {
                    path = '/' + path
                        .Replace('\\', '/')
                        .Replace("", "");
                }
                if (isSuccess)
                {
                    print.WriteLine(path, EPrintColor.Yellow);
                }
                else
                {
                    print.WriteLine(path, EPrintColor.Red);
                    print.WriteIntervalLine();
                }
            }
        }

        private ITask[] GetNeedExecuteITask(IPrintColor print, CommandOptions commandOptions)
        {
            return new ITask[]
            {
                new TaskGitRepoCheckStatus(log, encoding, print, commandOptions),
                new TaskGitRepoOpenShell(log, encoding, print, commandOptions),
                new TaskGitRepoRunCommand(log, encoding, print, commandOptions),
            };
        }
    }
}
