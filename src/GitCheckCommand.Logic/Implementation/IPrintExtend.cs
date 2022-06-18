using System;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
{
    /// <summary>
    /// ��̬��չ�ӿ�: ��ӡ����ӿ�
    /// </summary>
    public static class IPrintExtend
    {
        /// <summary>
        /// Ĭ�Ϻڵ�, д������
        /// </summary>
        /// <param name="printColor">��ӡ����ӿں�����ɫ��</param>
        /// <param name="content">��Ϣ����</param>
        /// <param name="textColor">�ı�������ɫ</param>
        public static void Write(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.Write(content, textColor, EPrintColor.None);
        }

        /// <summary>
        /// Ĭ�Ϻڵ�, д��һ������
        /// </summary>
        /// <param name="printColor">��ӡ����ӿں�����ɫ��</param>
        /// <param name="content">��Ϣ����</param>
        /// <param name="textColor">�ı�������ɫ</param>
        public static void WriteLine(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.WriteLine(content, textColor, EPrintColor.None);
        }

        private readonly static string interval_line = $"\n{"".PadLeft(80, '-')}\n";
        /// <summary>
        /// д������
        /// </summary>
        /// <param name="print">����ӿ�</param>
        public static void WriteIntervalLine(this IPrint print)
        {
            print.WriteLine(interval_line);
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
                    throw new ArgumentOutOfRangeException(nameof(systemType), $"����洢��·����ַ, �޷�����: {systemType}");
            }
        }
        private static void WriteOnlyGitRepositoryPath_WindowAndLinux(this IPrintColor print, bool IsClean, string PathFullName)
        {
            print.WriteLine(PathFullName, IsClean ? EPrintColor.Yellow : EPrintColor.Red);
        }
        private static void WriteOnlyGitRepositoryPath_WindowGitBash(this IPrintColor print, bool IsClean, string PathFullName)
        {
            string windowPath = PathFullName;
            string linuePath = '/' + PathFullName.Replace('\\', '/').Replace(":", "");
            print.WriteLine(linuePath, IsClean ? EPrintColor.Yellow : EPrintColor.Red);
            print.Write(" | ");
            print.WriteLine(windowPath, EPrintColor.Blue);
        }

        public static void WriteGitRepositoryStatusInfo(this IPrintColor print, GitRepositoryStatus status)
        {
            if (status.IsClean)
                return;
            print.WriteLine("��ǰ�ֿ���Ҫ����:\n");
            for (int i = 0; i < status.StatusMsgs.Length; i++)
            {
                string msg = status.StatusMsgs[i];
                if (i == status.NoCleanMsgIndex)
                {
                    print.WriteLine(msg, EPrintColor.Purple);
                    continue;
                }
                print.WriteLine(msg);
            }
        }
    }
}
