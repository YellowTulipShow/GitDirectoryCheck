using System.IO;

namespace YTS.Git
{
    /// <summary>
    /// 存储库
    /// </summary>
    public struct Repository
    {
        /// <summary>
        /// 所在系统目录地址
        /// </summary>
        public DirectoryInfo RootPath { get; set; }
    }

    /// <summary>
    /// 存储库 - 远程仓库
    /// </summary>
    public struct RepositoryRemoteWarehouse
    {
        /// <summary>
        /// 远程仓库标识名称
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// 远程分支名称
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 远程仓库地址
        /// </summary>
        public string Address { get; set; }
    }
}
