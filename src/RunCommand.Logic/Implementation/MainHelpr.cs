using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using YTS.Log;

using RunCommand.Logic.Models;
using System.Text.RegularExpressions;

namespace RunCommand.Logic.Implementation
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

        private CommandOptions commandOptions;
        private IPrintColor print;
        private Configs configs;
        public void OnExecute(string configFilePath, CommandOptions commandOptions)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["configFilePath"] = configFilePath;
            logArgs["commandOptions"] = commandOptions;
            this.commandOptions = commandOptions;

            print = commandOptions.SystemType.ToIPrintColor();
            string printTypeName = print.GetType().Name;
            logArgs["printTypeName"] = printTypeName;

            configs = new ConfigHelper(log, encoding, print)
                .ReadConfigs(configFilePath, this.commandOptions.SystemType);

            GitRepository[] gitRepos = new FindGitRepositoryHelper(log)
                .OnExecute(configs);
            foreach (GitRepository gitRepo in gitRepos)
            {
                print.WriteLine($"Git目录地址: [{gitRepo.Path.FullName}]");
            }

            //ITask[] tasks = GetNeedExecuteITask();
            //foreach (ITask task in tasks)
            //{
            //}

            log.Info("信息输出:", logArgs);
        }

        private ITask[] GetNeedExecuteITask()
        {
            return new ITask[]
            {
            };
        }
    }
}
