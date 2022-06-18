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
        private readonly ConfigHelper configHelper;
        private readonly FindGitRepositoryHelper findGitRepositoryHelper;

        public MainHelpr(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
            configHelper = new ConfigHelper(log, encoding);
            findGitRepositoryHelper = new FindGitRepositoryHelper(log);
        }

        public void OnExecute(string configFilePath, CommandOptions cOption)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = cOption;
            IPrintColor print = cOption.SystemType.ToIPrintColor();
            string printTypeName = print.GetType().Name;
            logArgs["printTypeName"] = printTypeName;
            Configs configs = configHelper.ReadConfigs(configFilePath, cOption.SystemType);
            GitRepository[] gitRepos = findGitRepositoryHelper.OnExecute(configs);
            IWriteGitRepositoryInfo[] readTools = GetNeed_IReadGitRepositoryInfo();
            ITask[] tasks = GetNeedExecuteITask(print, cOption);
            for (int index_gitRepo = 0; index_gitRepo < gitRepos.Length; index_gitRepo++)
            {
                GitRepository gitRepo = gitRepos[index_gitRepo];
                logArgs["gitRepo.Path.FullName"] = gitRepo.Path.FullName;
                foreach (var readTool in readTools)
                {
                    gitRepo = readTool.OnExecute(gitRepo);
                }
                HandlerGitRepository(print, gitRepo, tasks, cOption);
            }
        }
        private IWriteGitRepositoryInfo[] GetNeed_IReadGitRepositoryInfo()
        {
            return new IWriteGitRepositoryInfo[]
            {
                new WriteGitRepositoryInfo_Branch(log, encoding),
                new WriteGitRepositoryInfo_Status(log, encoding),
            };
        }

        private ITask[] GetNeedExecuteITask(IPrintColor print, CommandOptions commandOptions)
        {
            return new ITask[]
            {
                new Task_OpenShell(log, encoding, print, commandOptions),
                new Task_RunCommand(log, encoding, print, commandOptions),
            };
        }

        private void HandlerGitRepository(IPrintColor print, GitRepository gitRepo, ITask[] tasks, CommandOptions cOption)
        {
            var logArgs = log.CreateArgDictionary();
            if (gitRepo.Status.IClean)
            {
                print.WriteGitRepositoryPath(gitRepo, cOption.SystemType);
                return;
            }
            print.WriteGitRepositoryStatusInfo(gitRepo.Status);
            print.WriteIntervalLine();
            foreach (ITask task in tasks)
            {
                string taskTypeName = task.GetType().Name;
                logArgs["taskTypeName"] = taskTypeName;
                string taskDesc = task.GetDescribe();
                logArgs["taskDesc"] = taskDesc;
                TaskResponse response = task.OnExecute(gitRepo);
                if (!response.IsSuccess)
                {
                    print.WriteLine($"任务: [{taskDesc}]({taskTypeName}) 执行失败!");
                    print.WriteLine($"响应内容: [{response.ErrorCode}] {response.ErrorMessage}");
                    log.Error("任务执行失败!", logArgs);
                }
                else if (response.ErrorCode != ETaskResponseErrorCode.None)
                {
                    print.WriteLine($"任务: [{taskDesc}]({taskTypeName}) 执行完成!");
                    print.WriteLine($"响应内容: [{response.ErrorCode}] {response.ErrorMessage}");
                }
                else
                {
                    continue;
                }
                print.WriteLine("");
            }
            print.WriteGitRepositoryPath(gitRepo, cOption.SystemType);
        }
    }
}
