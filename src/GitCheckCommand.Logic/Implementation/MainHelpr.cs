using System.Text;

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
            int notCleanGitCount = 0;
            print.WriteIntervalLine();
            print.WriteLine("开始检查:");
            print.WriteIntervalLine();
            for (int index_gitRepo = 0; index_gitRepo < gitRepos.Length; index_gitRepo++)
            {
                GitRepository gitRepo = gitRepos[index_gitRepo];
                logArgs["gitRepo.Path.FullName"] = gitRepo.Path.FullName;
                foreach (var readTool in readTools)
                {
                    gitRepo = readTool.OnExecute(gitRepo);
                }
                if (!gitRepo.Status.IsClean)
                {
                    notCleanGitCount++;
                }
                HandlerGitRepository(print, gitRepo, tasks, cOption);
            }
            print.WriteIntervalLine();
            if (notCleanGitCount > 0)
            {
                print.Write("Need Oper Repo Count: ");
                print.WriteLine($"{notCleanGitCount}", EPrintColor.Red);
            }
            else
            {
                print.WriteLine("All warehouses are very clean... ok!", EPrintColor.Green);
            }
            print.WriteIntervalLine();
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
            if (gitRepo.Status.IsClean)
            {
                print.WriteGitRepositoryPath(gitRepo, cOption.SystemType);
                return;
            }
            print.WriteIntervalLine();
            print.WriteGitRepositoryStatusInfo(gitRepo.Status);
            print.WriteSpaceLine();
            foreach (ITask task in tasks)
            {
                string taskTypeName = task.GetType().Name;
                logArgs["taskTypeName"] = taskTypeName;
                string taskDesc = task.GetDescribe();
                logArgs["taskDesc"] = taskDesc;
                TaskResponse response = task.OnExecute(gitRepo);

                if (response.Code == ETaskResponseCode.None)
                    continue;
                if (response.IsSuccess)
                {
                    print.WriteLine($"任务: [{taskDesc}] ({taskTypeName}) [{response.Code}] 执行完成!");
                    if (!string.IsNullOrEmpty(response.ErrorMessage))
                        print.WriteLine($"错误消息: {response.ErrorMessage}");
                }
                else
                {
                    print.WriteLine($"任务: [{taskDesc}] ({taskTypeName}) [{response.Code}] 执行失败!");
                    if (!string.IsNullOrEmpty(response.ErrorMessage))
                        print.WriteLine($"错误消息: {response.ErrorMessage}");
                    log.Error("任务执行失败!", logArgs);
                }
                print.WriteSpaceLine();
            }
            print.WriteSpaceLine();
            print.WriteGitRepositoryPath(gitRepo, cOption.SystemType);
            print.WriteIntervalLine();
        }
    }
}
