namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 任务执行响应结果
    /// </summary>
    public struct TaskResponse
    {
        public bool IsSuccess { get; set; }
        public ETaskResponseErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
