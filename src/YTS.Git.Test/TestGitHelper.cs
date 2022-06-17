using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using YTS.Log;
using System.Diagnostics;
using System.Linq;

namespace YTS.Git.Test
{
    [TestClass]
    public class TestGitHelper
    {
        public const string root_dire = @"D:\_yts_git_test_run_dire";
        private ILog log;

        [TestInitialize]
        public void Init()
        {
            var logFile = new FileInfo($"./logs/TestGitHelper/{DateTime.Now:yyyy_MM_dd}.log");
            log = new FilePrintLog(logFile, Encoding.UTF8);
        }

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void Test_ALL()
        {
            // 未完全实现
            if (Directory.Exists(root_dire))
            {
                Directory.Delete(root_dire, true);
                Directory.CreateDirectory(root_dire);
            }

            IGit git = new GitHelper(new Repository()
            {
                RootPath = new DirectoryInfo(root_dire),
            });

            git.Init().OnCommand();
            Assert.AreEqual(true, Directory.Exists(root_dire));
            Assert.AreEqual(true, Directory.Exists(Path.Combine(root_dire, ".git")));

            string json = JsonConvert.SerializeObject(new { });
            File.WriteAllText(Path.Combine(root_dire, "1.json"), json);

            git.Add().OnCommand("1.json");

            IList<string> msgs;
            msgs = git.Status().OnCommand();
            msgs.Test(new string[] {
                "On branch master",
                "No commits yet",
                "Changes to be committed:",
                "  (use \"git rm --cached <file>...\" to unstage)",
                "        new file:   1.json",
            });

            git.Commit().OnCommand("save data");
            msgs = git.Status().OnCommand();
            msgs.Test(new string[] {
                "On branch master",
                "nothing to commit, working tree clean",
            });
        }
    }

    public static class StringListAssert
    {
        /// <summary>
        /// 是否测试通过
        /// </summary>
        /// <param name="actual">答案</param>
        /// <param name="expected">预期</param>
        public static void Test(this IList<string> actual, IList<string> expected)
        {
            Assert.IsNotNull(actual);
            Assert.IsNotNull(expected);
            actual = actual
                .Select(b => b.Trim())
                .Where(b => !string.IsNullOrEmpty(b))
                .ToList();
            expected = expected
                .Select(b => b.Trim())
                .Where(b => !string.IsNullOrEmpty(b))
                .ToList();
            const string sign = " | ";
            string actualStr = string.Join(sign, actual);
            string expectedStr = string.Join(sign, expected);
            Regex reSpace = new Regex(@"\s{2,}");
            actualStr = reSpace.Replace(actualStr, " ");
            expectedStr = reSpace.Replace(expectedStr, " ");
            Assert.IsTrue(actualStr.Length > 4);
            Assert.IsTrue(expectedStr.Length > 4);
            Assert.AreEqual(actualStr, expectedStr);
        }
    }
}


/*

$ git status
On branch master

No commits yet

Changes to be committed:
  (use "git rm --cached <file>..." to unstage)
        new file:   1.json


$ git commit -m "save data"
[master (root-commit) d086173] save data
 1 file changed, 1 insertion(+)
 create mode 100644 1.json



$ git status
On branch master
nothing to commit, working tree clean



$ git push -u origin "master"
Enumerating objects: 3, done.
Counting objects: 100% (3/3), done.
Writing objects: 100% (3/3), 218 bytes | 72.00 KiB/s, done.
Total 3 (delta 0), reused 0 (delta 0), pack-reused 0
remote: Powered by GITEE.COM [GNK-6.3]
To gitee.com:YellowTulipShow/PlanNotes.YTSZRQ.StorageArea.git
 * [new branch]      master -> master
Branch 'master' set up to track remote branch 'master' from 'origin'.



$ git status
On branch master
Your branch is up to date with 'origin/master'.

nothing to commit, working tree clean



$ git pull origin master
From gitee.com:YellowTulipShow/PlanNotes.YTSZRQ.StorageArea
 * branch            master     -> FETCH_HEAD
Already up to date.



$ git pull origin master
remote: Enumerating objects: 4, done.
remote: Counting objects: 100% (4/4), done.
remote: Compressing objects: 100% (2/2), done.
remote: Total 3 (delta 0), reused 0 (delta 0), pack-reused 0
Unpacking objects: 100% (3/3), 945 bytes | 94.00 KiB/s, done.
From gitee.com:YellowTulipShow/PlanNotes.YTSZRQ.StorageArea
 * branch            master     -> FETCH_HEAD
   d086173..7d99e9a  master     -> origin/master
Updating d086173..7d99e9a
Fast-forward
 2.txt | 1 +
 1 file changed, 1 insertion(+)
 create mode 100644 2.txt



$ git status
On branch master
Your branch is up to date with 'origin/master'.

Untracked files:
  (use "git add <file>..." to include in what will be committed)
        3.txt

nothing added to commit but untracked files present (use "git add" to track)



$ git add .
warning: LF will be replaced by CRLF in 3.txt.
The file will have its original line endings in your working directory





$ git commit -m "保存"
[master 1aed01b] 保存
 1 file changed, 1 insertion(+)
 create mode 100644 3.txt



$ git status
On branch master
Your branch is ahead of 'origin/master' by 1 commit.
  (use "git push" to publish your local commits)

nothing to commit, working tree clean




$ git push origin master
Enumerating objects: 4, done.
Counting objects: 100% (4/4), done.
Delta compression using up to 4 threads
Compressing objects: 100% (2/2), done.
Writing objects: 100% (3/3), 343 bytes | 343.00 KiB/s, done.
Total 3 (delta 0), reused 0 (delta 0), pack-reused 0
remote: Powered by GITEE.COM [GNK-6.3]
To gitee.com:YellowTulipShow/PlanNotes.YTSZRQ.StorageArea.git
   7d99e9a..1aed01b  master -> master




 */