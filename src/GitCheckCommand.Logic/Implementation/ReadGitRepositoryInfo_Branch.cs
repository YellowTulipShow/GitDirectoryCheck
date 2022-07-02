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
    public class ReadGitRepositoryInfo_Branch : IReadGitRepositoryInfo
    {
        private readonly ILog log;
        private readonly Encoding encoding;

        /// <summary>
        /// 实例化 - 写入存储库信息实现类: 分支信息
        /// </summary>
        /// <param name="log">日志接口</param>
        /// <param name="encoding">文本编码</param>
        public ReadGitRepositoryInfo_Branch(ILog log, Encoding encoding)
        {
            this.log = log;
            this.encoding = encoding;
        }

        /// <inheritdoc/>
        public GitRepository OnExecute(GitRepository repository)
        {
            var gitBranch = new ReadNameClass(log, new Repository()
            {
                OutputTextEncoding = encoding,
                RootPath = repository.Path,
            });
            repository.BranchName = gitBranch.GetSelfBranchName();
            return repository;
        }

        internal class ReadNameClass : AbsGitCommandExecuteHelper
        {
            private readonly ILog log;
            public ReadNameClass(ILog log, Repository repository) : base(repository)
            {
                this.log = log;
            }

            public string GetSelfBranchName()
            {
                var logArgs = log.CreateArgDictionary();
                IList<string> lines = new List<string>() { };
                logArgs["lines"] = lines;
                OnExecuteOnlyCommand("branch", line =>
                {
                    lines.Add(line);
                });
                Regex selfBranchRegex = new Regex(@"^\*\s+([^\s]+)$");
                logArgs["selfBranchRegex"] = selfBranchRegex.ToString();
                foreach (string line in lines)
                {
                    logArgs["line"] = line;
                    Match match = selfBranchRegex.Match(line);
                    if (match.Success)
                    {
                        return match.Groups[1].Value;
                    }
                }
                log.Error("无法查询到当前分支名称", logArgs);
                return @"<Query NULL Branch!!!>";
            }
        }
    }
}
