using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using RunCommand.Logic.Models;

using YTS.Log;

namespace RunCommand.Logic.Implementation
{
    public class FindGitRepositoryHelper
    {
        private readonly ILog log;
        private int depthCount;

        public FindGitRepositoryHelper(ILog log)
        {
            this.log = log;
        }

        public GitRepository[] OnExecute(Configs configs)
        {
            depthCount = 0;
            var logArgs = log.CreateArgDictionary();
            List<GitRepository> gitRepos = new List<GitRepository>();
            foreach (ConfigRoot croot in configs.Roots)
            {
                string rootPath = croot.Path;
                logArgs["rootPath"] = rootPath;
                DirectoryInfo rootDire = new DirectoryInfo(rootPath);
                if (!rootDire.Exists)
                {
                    log.Error("配置项中根目录不存在!", logArgs);
                    continue;
                }
                var ignoresRegexs = (croot.IgnoresRegexs ?? new string[] { })
                    .Concat(configs.IgnoresRegexs ?? new string[] { });
                var gits_list = DepthFind(rootDire, ignoresRegexs);
                depthCount -= 1;
                if (gits_list != null)
                {
                    gitRepos.AddRange(gits_list);
                }
            }
            if (depthCount != 0)
            {
                throw new Exception($"查询深度数量异常: {depthCount}");
            }
            return gitRepos.ToArray();
        }

        private IList<GitRepository> DepthFind(DirectoryInfo rootDire, IEnumerable<string> ignoresRegexs)
        {
            depthCount += 1;
            const int max_depth = 999;
            if (depthCount >= max_depth)
            {
                throw new Exception($"深度查询限制: {max_depth} 次, 已超!");
            }
            List<GitRepository> gitRepos = new List<GitRepository>();
            if (rootDire == null)
            {
                return gitRepos;
            }
            // 判断是否时忽略路径
            foreach (string ignoresRegex in ignoresRegexs)
            {
                if (Regex.IsMatch(rootDire.FullName, ignoresRegex,
                    RegexOptions.ECMAScript | RegexOptions.IgnoreCase))
                {
                    return gitRepos;
                }
            }
            var subDires = rootDire.GetDirectories();
            if (subDires.Any(d => d.Name.ToLower() == ".git"))
            {
                gitRepos.Add(ToGitRepository(rootDire));
                return gitRepos;
            }
            foreach (DirectoryInfo subDire in subDires)
            {
                if (subDire.FullName == rootDire.FullName)
                {
                    continue;
                }
                var subGitRepos = DepthFind(subDire, ignoresRegexs);
                depthCount -= 1;
                gitRepos.AddRange(subGitRepos);
            }
            return gitRepos;
        }

        private GitRepository ToGitRepository(DirectoryInfo gitDireInfo)
        {
            return new GitRepository()
            {
                Path = gitDireInfo,
            };
        }
    }
}
