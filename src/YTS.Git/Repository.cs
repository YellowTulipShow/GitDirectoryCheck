namespace YTS.Git
{
    /// <summary>
    /// 存储库
    /// </summary>
    public struct Repository
    {
        /// <summary>
        /// 系统地址
        /// </summary>
        public string SystemPath { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 存储分支名称
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 远程仓库
        /// </summary>
        public RepositoryRemoteWarehouse Remote { get; set; }
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
