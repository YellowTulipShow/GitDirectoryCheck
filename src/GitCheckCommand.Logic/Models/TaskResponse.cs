namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 任务执行响应结果
    /// </summary>
    public struct TaskResponse
    {
        /// <summary>
        /// 响应编码枚举
        /// </summary>
        public ETaskResponseCode Code { get; set; }

        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 执行失败返回错误消息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
