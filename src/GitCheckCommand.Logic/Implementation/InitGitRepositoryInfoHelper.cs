using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using GitCheckCommand.Logic.Models;

using YTS.Log;

using YTS.Git.Implementation;
using YTS.Git.Models;

namespace GitCheckCommand.Logic.Implementation
{
    public class InitGitRepositoryInfoHelper
    {
        private readonly ILog log;
        private readonly Encoding encoding;
        private readonly CommandOptions commandOptions;

        public InitGitRepositoryInfoHelper(ILog log, Encoding encoding, CommandOptions commandOptions)
        {
            this.log = log;
            this.encoding = encoding;
            this.commandOptions = commandOptions;
        }

        public GitRepository OnExecute(GitRepository repository)
        {
            Repository repo = new Repository()
            {
                OutputTextEncoding = encoding,
                RootPath = repository.Path,
            };
            var gitBranch = new GitBanchGet(repo);
            repository.BranchName = gitBranch.GetSelfBranchName();
            return repository;
        }
    }
    internal class GitBanchGet : AbsGitCommandExecuteHelper
    {
        public GitBanchGet(Repository repository) : base(repository)
        {
        }

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
