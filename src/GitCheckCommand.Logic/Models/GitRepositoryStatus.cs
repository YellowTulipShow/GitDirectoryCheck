namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 存储库信息查询结果
    /// </summary>
    public struct GitRepositoryStatus
    {
        /// <summary>
        /// 存储库是否'干净'
        /// </summary>
        public bool IsClean { get; set; }

        /// <summary>
        /// 查询状态消息内容队列
        /// </summary>
        public string[] StatusMsgs { get; set; }

        /// <summary>
        /// 不'干净'判断依据消息序列标识
        /// </summary>
        public int NoCleanMsgIndex { get; set; }
    }
}
