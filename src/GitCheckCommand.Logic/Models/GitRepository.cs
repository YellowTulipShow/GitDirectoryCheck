using System.IO;

namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 存储库信息
    /// </summary>
    public struct GitRepository
    {
        /// <summary>
        /// 存储库根目录路径
        /// </summary>
        public DirectoryInfo Path { get; set; }

        /// <summary>
        /// 是否默认需要打开Shell
        /// </summary>
        public bool? IsOpenShell { get; set; }

        /// <summary>
        /// 当前分支名称
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 存储库信息查询结果
        /// </summary>
        public GitRepositoryStatus Status { get; set; }
    }
}
