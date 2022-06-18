namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 任务执行响应结果 - 错误代码
    /// </summary>
    public enum ETaskResponseErrorCode
    {
        /// <summary>
        /// 无错误
        /// </summary>
        None,

        /// <summary>
        /// 参数为空错误
        /// </summary>
        ParameterIsEmpty,

        /// <summary>
        /// 未完待续
        /// </summary>
        NotFinished,
    }
}
