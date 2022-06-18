using System;
using System.Collections.Generic;
using System.Text;

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
        /// 仓库不干净需要处理保存
        /// </summary>
        NotClean,

        /// <summary>
        /// 未完待续
        /// </summary>
        NotFinished,
    }
}
