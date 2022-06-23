using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using System.CommandLine;

using YTS.Log;
using YTS.ConsolePrint;

using GitCheckCommand.Logic;
using GitCheckCommand.Logic.Models;

namespace GitCheckCommand
{
    /// <summary>
    /// 命令参数解析器
    /// </summary>
    public class CommandArgsParser
    {
        private readonly ILog log;
        private readonly IMain main;

        /// <summary>
        /// 实例化 - 命令参数解析器
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="main">主程序接口</param>
        public CommandArgsParser(ILog log, IMain main)
        {
            this.log = log;
            this.main = main;
        }

        /// <summary>
        /// 解析执行
        /// </summary>
        /// <param name="args">用户传入的命令行参数</param>
        /// <returns>执行返回编码</returns>
        public int OnParser(string[] args)
        {
            var logArgs = log.CreateArgDictionary();
            logArgs["CommandInputArgs"] = args;
            int code = 0;
            try
            {
                Option<string> configFilePathOption = GetOption_ConfigFilePath();
                Option<EConsoleType> systemTypeOption = GetOption_ConsoleType();

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
                        EConsoleType consoleType = context.ParseResult.GetValueForOption(systemTypeOption);
                        logArgs["consoleType"] = consoleType.ToString();

                        bool? isOpenShell = context.ParseResult.GetValueForOption(openShellOption);
                        logArgs["isOpenShell"] = isOpenShell;
                        string command = context.ParseResult.GetValueForOption(commandOption);
                        logArgs["command"] = command;
                        main.OnExecute(configFilePath, new Logic.Models.CommandOptions()
                        {
                            ConsoleType = consoleType,
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
        private Option<EConsoleType> GetOption_ConsoleType()
        {
            var option = new Option<EConsoleType>(
                aliases: new string[] { "--console" },
                description: "配置当前执行的控制台标识, 用于打印输出消息内容颜色使用",
                getDefaultValue: () => EConsoleTypeExtend.GetDefalutConsoleType());
            option.Arity = ArgumentArity.ExactlyOne;
            return option;
        }
        private Option<bool?> GetOption_OpenShell()
        {
            var option = new Option<bool?>(
                aliases: new string[] { "-o", "--open-shell" },
                getDefaultValue: () => null,
                description: "当Git仓库'不干净'时, 是否需要自动打开命令窗口"); ;
            option.Arity = ArgumentArity.ZeroOrOne;
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
