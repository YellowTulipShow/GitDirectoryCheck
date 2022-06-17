using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using YTS.Git.SubCommands;

namespace YTS.Git
{
    /// <summary>
    /// Git 操作工具类
    /// </summary>
    public class GitHelper : IGit, IGitInit, IGitAdd, IGitStatus, IGitCommit, IGitPull, IGitPush
    {
        private readonly Encoding encoding;
        private readonly Repository repository;

        /// <summary>
        /// 实例化 - Git 操作工具类
        /// </summary>
        /// <param name="repository">需操作的存储库配置</param>
        public GitHelper(Repository repository)
        {
            this.repository = repository;
            encoding = Encoding.UTF8;
        }

        private void OnExecuteOnlyCommand(string command, Action<string> readLineCallBack)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"git", command)
            {
                UseShellExecute = false,
                WorkingDirectory = repository.SystemPath,
                RedirectStandardOutput = true,
                StandardOutputEncoding = encoding,
            };
            using Process process = Process.Start(info);
            if (readLineCallBack != null)
            {
                using StreamReader sr = process.StandardOutput;
                while (!sr.EndOfStream)
                {
                    readLineCallBack(sr.ReadLine());
                }
                sr.Close();
            }
            process.WaitForExit();
            process.Close();
        }

        #region IGit
        /// <inheritdoc />
        IGitInit IGit.Init() => this;
        /// <inheritdoc />
        IGitAdd IGit.Add() => this;
        /// <inheritdoc />
        IGitStatus IGit.Status() => this;
        /// <inheritdoc />
        IGitCommit IGit.Commit() => this;
        /// <inheritdoc />
        IGitPull IGit.Pull() => this;
        /// <inheritdoc />
        IGitPush IGit.Push() => this;
        #endregion

        #region IGitInit
        /// <inheritdoc />
        void IGitInit.OnCommand()
        {
            DirectoryInfo dire = new DirectoryInfo(repository.SystemPath);
            if (!dire.Exists)
            {
                dire.Create();
            }
            if (!dire.GetDirectories().Any(b => b.Name.ToLower() == @".git"))
            {
                OnExecuteOnlyCommand($"init", null);
            }
        }
        #endregion

        #region IGitAdd
        /// <inheritdoc />
        void IGitAdd.OnCommand()
        {
            OnExecuteOnlyCommand($"add .", null);
        }
        /// <inheritdoc />
        void IGitAdd.OnCommand(string filePath)
        {
            OnExecuteOnlyCommand($"add {filePath}", null);
        }
        /// <inheritdoc />
        void IGitAdd.OnCommand(IList<string> fileNames)
        {
            OnExecuteOnlyCommand($"add {string.Join(' ', fileNames)}", null);
        }
        #endregion

        #region IGitStatus
        /// <inheritdoc />
        IList<string> IGitStatus.OnCommand()
        {
            IList<string> lines = new List<string>();
            OnExecuteOnlyCommand("status", line =>
            {
                lines.Add(line);
            });
            return lines.ToArray();
        }
        #endregion

        #region IGitCommit
        /// <inheritdoc />
        void IGitCommit.OnCommand(string message)
        {
            OnExecuteOnlyCommand($"commit -m \"{message}\"", null);
        }
        #endregion

        #region IGitPull
        /// <inheritdoc />
        IList<string> IGitPull.OnCommand(string message)
        {
            IList<string> lines = new List<string>();
            OnExecuteOnlyCommand($"fetch origin", line =>
            {
                lines.Add(line);
            });
            OnExecuteOnlyCommand($"merge origin/master -m \"{message}\"", line =>
            {
                lines.Add(line);
            });
            return lines.ToArray();
        }
        /// <inheritdoc />
        IList<string> IGitPull.OnCommand(string origin, string branch, string message)
        {
            IList<string> lines = new List<string>();
            OnExecuteOnlyCommand($"fetch {origin}", line =>
            {
                lines.Add(line);
            });
            OnExecuteOnlyCommand($"merge {origin}/{branch} -m \"{message}\"", line =>
            {
                lines.Add(line);
            });
            return lines.ToArray();
        }
        #endregion

        #region IGitPush
        /// <inheritdoc />
        IList<string> IGitPush.OnCommand()
        {
            IList<string> lines = new List<string>();
            OnExecuteOnlyCommand($"push origin master", line =>
            {
                lines.Add(line);
            });
            return lines.ToArray();
        }
        /// <inheritdoc />
        IList<string> IGitPush.OnCommand(string origin, string branch)
        {
            IList<string> lines = new List<string>();
            OnExecuteOnlyCommand($"push {origin} {branch}", line =>
            {
                lines.Add(line);
            });
            return lines.ToArray();
        }
        #endregion
    }
}
