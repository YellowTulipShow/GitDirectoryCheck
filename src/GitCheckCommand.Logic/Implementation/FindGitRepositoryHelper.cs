using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using YTS.Log;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic.Implementation
{
    /// <summary>
    /// 查找存储库帮助类
    /// </summary>
    public class FindGitRepositoryHelper
    {
        private readonly ILog log;
        private int depthCount;

        /// <summary>
        /// 实例化 - 查找存储库帮助类
        /// </summary>
        /// <param name="log">日志接口</param>
        public FindGitRepositoryHelper(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// 执行查找
        /// </summary>
        /// <param name="configs">系统配置项</param>
        /// <returns>存储库名单列表</returns>
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
                bool? isOpenShell = croot.IsOpenShell ?? configs.IsOpenShell;
                var gits_list = DepthFind(rootDire, ignoresRegexs, isOpenShell);
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

        private IList<GitRepository> DepthFind(DirectoryInfo rootDire, IEnumerable<string> ignoresRegexs, bool? isOpenShell)
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
                // 写入 Git 仓库相关执行信息
                gitRepos.Add(new GitRepository()
                {
                    Path = rootDire,
                    IsOpenShell = isOpenShell,
                });
                return gitRepos;
            }
            foreach (DirectoryInfo subDire in subDires)
            {
                if (subDire.FullName == rootDire.FullName)
                {
                    continue;
                }
                var subGitRepos = DepthFind(subDire, ignoresRegexs, isOpenShell);
                depthCount -= 1;
                gitRepos.AddRange(subGitRepos);
            }
            return gitRepos;
        }
    }
}
