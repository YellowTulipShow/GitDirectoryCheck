using System.Text;

using YTS.Log;
using YTS.ConsolePrint;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 主程序帮助类
    /// </summary>
    public class MainHelpr : IMain
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly FindGitRepositoryHelper findGitRepositoryHelper;

        /// <summary>
        /// 实例化 - 主程序帮助类
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        public MainHelpr(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
            findGitRepositoryHelper = new FindGitRepositoryHelper(log);
        }

        /// <summary>
        /// 执行逻辑程序
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="cOption">用户传入的命令选项</param>
        public void OnExecute(string configFilePath, CommandOptions cOption)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = cOption;
            IPrintColor print = cOption.ConsoleType.ToIPrintColor();
            string printTypeName = print.GetType().Name;
            logArgs["printTypeName"] = printTypeName;

            var configHelper = new ConfigHelper(log, encoding, print);
            Configs configs = configHelper.ReadConfigs(configFilePath, cOption.ConsoleType);
            GitRepository[] gitRepos = findGitRepositoryHelper.OnExecute(configs);
            if (gitRepos == null || gitRepos.Length <= 0)
            {
                print.WriteLine("存储库列表为空!", EPrintColor.Red);
                return;
            }
            IWriteGitRepositoryInfo[] readTools = GetNeed_IReadGitRepositoryInfo();
            ITask[] tasks = GetNeedExecuteITask(print, cOption, configs);
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

        private ITask[] GetNeedExecuteITask(IPrintColor print, CommandOptions commandOptions, Configs configs)
        {
            return new ITask[]
            {
                new Task_OpenShell(log, print, commandOptions, configs),
                new Task_RunCommand(log, print, commandOptions),
            };
        }

        private void HandlerGitRepository(IPrintColor print, GitRepository gitRepo, ITask[] tasks, CommandOptions cOption)
        {
            var logArgs = log.CreateArgDictionary();
            if (gitRepo.Status.IsClean)
            {
                print.WriteGitRepositoryPath(gitRepo, cOption.ConsoleType);
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
            print.WriteGitRepositoryPath(gitRepo, cOption.ConsoleType);
            print.WriteIntervalLine();
        }
    }
}
