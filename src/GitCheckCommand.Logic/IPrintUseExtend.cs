using System;
using System.Text.RegularExpressions;

using GitCheckCommand.Logic.Models;

using YTS.ConsolePrint;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// 静态扩展接口: 打印输出接口
    /// </summary>
    public static class IPrintUseExtend
    {
        private readonly static string interval_line = $"{"".PadLeft(80, '-')}";
        private static int beforeWriteIntervalLineCount = -1;
        /// <summary>
        /// 写入间隔行
        /// </summary>
        /// <param name="print">输出接口</param>
        public static void WriteIntervalLine(this IPrint print)
        {
            if (beforeWriteIntervalLineCount == print.GetLineCount())
            {
                return;
            }
            print.WriteLine(interval_line);
            beforeWriteIntervalLineCount = print.GetLineCount();
        }

        private static int beforeWriteSpaceLineCount = -1;
        /// <summary>
        /// 写入空行
        /// </summary>
        /// <param name="print">输出接口</param>
        public static void WriteSpaceLine(this IPrint print)
        {
            if (beforeWriteSpaceLineCount == print.GetLineCount())
            {
                return;
            }
            print.WriteLine(string.Empty);
            beforeWriteSpaceLineCount = print.GetLineCount();
        }

        /// <summary>
        /// 写入存储库路径信息行
        /// </summary>
        /// <param name="print">输出接口</param>
        /// <param name="gitRepo">存储库信息</param>
        /// <param name="consoleType">系统类型</param>
        public static void WriteGitRepositoryPath(this IPrintColor print, GitRepository gitRepo, EConsoleType consoleType)
        {
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
            switch (consoleType)
            {
                case EConsoleType.CMD:
                case EConsoleType.PowerShell:
                case EConsoleType.Bash:
                    print.WriteOnlyGitRepositoryPath_WindowAndLinux(gitRepo.Status.IsClean, gitRepo.Path.FullName);
                    break;
                case EConsoleType.WindowGitBash:
                    print.WriteOnlyGitRepositoryPath_WindowGitBash(gitRepo.Status.IsClean, gitRepo.Path.FullName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(consoleType), $"输出存储库路径地址, 无法解析: {consoleType}");
            }
        }
        private static void WriteOnlyGitRepositoryPath_WindowAndLinux(this IPrintColor print, bool IsClean, string PathFullName)
        {
            print.Write(PathFullName, IsClean ? EPrintColor.Yellow : EPrintColor.Red);
            print.Write("\n");
        }
        private static void WriteOnlyGitRepositoryPath_WindowGitBash(this IPrintColor print, bool IsClean, string PathFullName)
        {
            string windowPath = PathFullName;
            string linuePath = '/' + PathFullName.Replace('\\', '/').Replace(":", "");
            print.Write(linuePath, IsClean ? EPrintColor.Yellow : EPrintColor.Red);
            print.Write(" | ");
            print.Write(windowPath, EPrintColor.Blue);
            print.Write("\n");
        }

        /// <summary>
        /// 写入存储库状态相关信息
        /// </summary>
        /// <param name="print">输出接口</param>
        /// <param name="status">存储库状态信息</param>
        public static void WriteGitRepositoryStatusInfo(this IPrintColor print, GitRepositoryStatus status)
        {
            if (status.IsClean)
                return;

            Regex spaceLineRegex = new Regex(@"^\s*$");
            print.WriteLine("当前仓库需要处理:\n");
            for (int i = 0; i < status.StatusMsgs.Length; i++)
            {
                string msg = status.StatusMsgs[i];
                if (i == status.NoCleanMsgIndex)
                {
                    print.WriteLine(msg, EPrintColor.Purple);
                    continue;
                }
                if (spaceLineRegex.IsMatch(msg) && i == status.StatusMsgs.Length - 1)
                {
                    continue;
                }
                print.WriteLine(msg);
            }
        }
    }
}
