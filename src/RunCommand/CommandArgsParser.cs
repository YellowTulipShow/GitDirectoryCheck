using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using System.CommandLine;

using YTS.Log;

namespace RunCommand
{
    public class CommandArgsParser
    {
        private readonly ILog log;
        private readonly MainHelpr mainHelpr;

        public CommandArgsParser(ILog log, MainHelpr mainHelpr)
        {
            this.log = log;
            this.mainHelpr = mainHelpr;
        }

        public int OnParser(string[] args)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["CommandInputArgs"] = args;
            int code = 0;
            try
            {
                Option<bool> openShellOption = GetOption_OpenShell();
                Option<string> commandOption = GetOption_Command();

                RootCommand rootC = new RootCommand("检查目录下所有 Git 仓库状态");
                rootC.AddOption(openShellOption);
                rootC.AddOption(commandOption);
                rootC.SetHandler(context =>
                {
                    try
                    {
                        bool isOpenShell = context.ParseResult.GetValueForOption(openShellOption);
                        logArgs["isOpenShell"] = isOpenShell;
                        string command = context.ParseResult.GetValueForOption(commandOption);
                        logArgs["command"] = command;
                        mainHelpr.OnExecute(isOpenShell, command);
                    }
                    catch (Exception ex)
                    {
                        log.Error("执行程序逻辑出错", ex, logArgs);
                        code = 2;
                    }
                });

                return rootC.Invoke(args);
            }
            catch (Exception ex)
            {
                log.Error("解释命令出错", ex, logArgs);
                code = 1;
            }
            return code;
        }

        private Option<bool> GetOption_OpenShell()
        {
            var option = new Option<bool>(
                aliases: new string[] { "-o", "--open-shell" },
                getDefaultValue: () => false,
                description: "当Git仓库'不干净'时, 是否需要自动打开命令窗口");;
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
        private Option<string> GetOption_Command()
        {
            var option = new Option<string>(
                aliases: new string[] { "-o", "--output" },
                getDefaultValue: () => null,
                description: "当Git仓库'不干净'时, 在其路径执行的命令内容");
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
    }
}
