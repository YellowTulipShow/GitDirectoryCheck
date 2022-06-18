using System;
using System.Text.RegularExpressions;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// 静态扩展接口: 打印输出接口
    /// </summary>
    public static class IPrintExtend
    {
        /// <summary>
        /// 默认黑底, 写入内容
        /// </summary>
        /// <param name="printColor">打印输出接口含有颜色项</param>
        /// <param name="content">消息内容</param>
        /// <param name="textColor">文本内容颜色</param>
        public static void Write(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.Write(content, textColor, EPrintColor.None);
        }

        /// <summary>
        /// 默认黑底, 写入一行内容
        /// </summary>
        /// <param name="printColor">打印输出接口含有颜色项</param>
        /// <param name="content">消息内容</param>
        /// <param name="textColor">文本内容颜色</param>
        public static void WriteLine(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.WriteLine(content, textColor, EPrintColor.None);
        }

        //private readonly static string interval_line = $"\n{"".PadLeft(80, '-')}\n";
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

        public static void WriteGitRepositoryPath(this IPrintColor print, GitRepository gitRepo, ESystemType systemType)
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
            switch (systemType)
            {
                case ESystemType.Window:
                    print.WriteOnlyGitRepositoryPath_WindowAndLinux(gitRepo.Status.IsClean, gitRepo.Path.FullName);
                    break;
                case ESystemType.Linux:
                    print.WriteOnlyGitRepositoryPath_WindowAndLinux(gitRepo.Status.IsClean, gitRepo.Path.FullName);
                    break;
                case ESystemType.WindowGitBash:
                    print.WriteOnlyGitRepositoryPath_WindowGitBash(gitRepo.Status.IsClean, gitRepo.Path.FullName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(systemType), $"输出存储库路径地址, 无法解析: {systemType}");
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
