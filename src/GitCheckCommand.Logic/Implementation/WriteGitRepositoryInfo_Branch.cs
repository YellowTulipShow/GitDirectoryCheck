using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using YTS.Log;

using YTS.Git.Implementation;
using YTS.Git.Models;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 写入存储库信息实现类: 分支信息
    /// </summary>
    public class WriteGitRepositoryInfo_Branch : IWriteGitRepositoryInfo
    {
        private readonly ILog log;
        private readonly Encoding encoding;

        /// <summary>
        /// 实例化 - 写入存储库信息实现类: 分支信息
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        public WriteGitRepositoryInfo_Branch(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
        }

        /// <inheritdoc/>
        public GitRepository OnExecute(GitRepository repository)
        {
            var gitBranch = new ReadNameClass(new Repository()
            {
                OutputTextEncoding = encoding,
                RootPath = repository.Path,
            });
            repository.BranchName = gitBranch.GetSelfBranchName();
            return repository;
        }

        internal class ReadNameClass : AbsGitCommandExecuteHelper
        {
            public ReadNameClass(Repository repository) : base(repository) { }

            public string GetSelfBranchName()
            {
                IList<string> lines = new List<string>();
                OnExecuteOnlyCommand("branch", line =>
                {
                    lines.Add(line);
                });
                Regex selfBranchRegex = new Regex(@"^\*\s+([^\s]+)$");
                foreach (string line in lines)
                {
                    Match match = selfBranchRegex.Match(line);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
                throw new Exception("无法查询到当前分支名称");
            }
        }
    }
}
