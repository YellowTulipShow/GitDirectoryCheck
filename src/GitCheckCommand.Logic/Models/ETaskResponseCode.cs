namespace GitCheckCommand.Logic.Models
{
    /// <summary>
    /// 枚举: 任务执行响应结果 - 代码标识
    /// </summary>
    public enum ETaskResponseCode
    {
        /// <summary>
        /// 无动作
        /// </summary>
        None,

        /// <summary>
        /// 参数为空
        /// </summary>
        ParameterIsEmpty,

        /// <summary>
        /// 参数错误
        /// </summary>
        ParameterError,

        /// <summary>
        /// 未完待续
        /// </summary>
        NotFinished,

        /// <summary>
        /// 结束
        /// </summary>
        End,

        /// <summary>
        /// 发生异常
        /// </summary>
        Exception,
    }
}
