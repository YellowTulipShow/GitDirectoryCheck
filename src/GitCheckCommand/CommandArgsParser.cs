using System;

using System.CommandLine;

using YTS.Log;

using GitCheckCommand.Logic;
using GitCheckCommand.Logic.Models;
using System.Runtime.InteropServices;
using System.IO;

namespace GitCheckCommand
{
    public class CommandArgsParser
    {
        private readonly ILog log;
        private readonly IMain main;

        public CommandArgsParser(ILog log, IMain main)
        {
            this.log = log;
            this.main = main;
        }

        public int OnParser(string[] args)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["CommandInputArgs"] = args;
            int code = 0;
            try
            {
                Option<string> configFilePathOption = GetOption_ConfigFilePath();
                Option<ESystemType> systemTypeOption = GetOption_SystemType();

                Option<bool?> openShellOption = GetOption_OpenShell();
                Option<string> commandOption = GetOption_Command();

                RootCommand rootC = new RootCommand("检查目录下所有 Git 仓库状态");
                rootC.AddGlobalOption(configFilePathOption);
                rootC.AddGlobalOption(systemTypeOption);

                rootC.AddOption(openShellOption);
                rootC.AddOption(commandOption);

                rootC.SetHandler(context =>
                {
                    try
                    {
                        string configFilePath = context.ParseResult.GetValueForOption(configFilePathOption);
                        logArgs["configFilePath"] = configFilePath;
                        ESystemType systemType = context.ParseResult.GetValueForOption(systemTypeOption);
                        logArgs["systemType"] = systemType.ToString();

                        bool? isOpenShell = context.ParseResult.GetValueForOption(openShellOption);
                        logArgs["isOpenShell"] = isOpenShell;
                        string command = context.ParseResult.GetValueForOption(commandOption);
                        logArgs["command"] = command;
                        main.OnExecute(configFilePath, new Logic.Models.CommandOptions()
                        {
                            SystemType = systemType,
                            IsOpenShell = isOpenShell,
                            Command = command,
                        });
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

        private Option<string> GetOption_ConfigFilePath()
        {
            var option = new Option<string>(
                aliases: new string[] { "-c", "--config" },
                getDefaultValue: () =>
                {
                    // 当前用户配置地址
                    string dire = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string file = Path.Combine(dire, ".command_gitcheck_config.json");
                    return file;
                },
                description: "配置文件读取路径"); ;
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
        private Option<ESystemType> GetOption_SystemType()
        {
            var option = new Option<ESystemType>(
                aliases: new string[] { "--system" },
                getDefaultValue: () =>
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        if (Console.Title.ToLower().Trim().Contains(@"invisible cygwin console"))
                        {
                            return ESystemType.Linux;
                        }
                        return ESystemType.Window;
                    }
                    return ESystemType.Linux;
                },
                description: "显示的执行当前执行的系统标识, 用于打印输出消息内容颜色使用");
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
        private Option<bool?> GetOption_OpenShell()
        {
            var option = new Option<bool?>(
                aliases: new string[] { "-o", "--open-shell" },
                getDefaultValue: () => null,
                description: "当Git仓库'不干净'时, 是否需要自动打开命令窗口"); ;
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
        private Option<string> GetOption_Command()
        {
            var option = new Option<string>(
                aliases: new string[] { "--command" },
                getDefaultValue: () => null,
                description: "当Git仓库'不干净'时, 在其路径执行的命令内容");
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
    }
}
