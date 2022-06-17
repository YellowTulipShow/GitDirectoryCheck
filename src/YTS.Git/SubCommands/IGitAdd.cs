using System.Collections.Generic;

namespace YTS.Git.SubCommands
{
    /// <summary>
    /// 接口: Git - status 子命令
    /// </summary>
    public interface IGitAdd
    {
        /// <summary>
        /// 执行命令: git add .
        /// </summary>
        void OnCommand();

        /// <summary>
        /// 执行命令: git add {fileName}
        /// </summary>
        /// <param name="fileName">文件路径</param>
        void OnCommand(string fileName);

        /// <summary>
        /// 执行命令: git add {fileNames[0]} {fileNames[1]} {fileNames[2]} ... {fileNames[n-1]}
        /// </summary>
        /// <param name="fileNames">文件路径清单</param>
        void OnCommand(IList<string> fileNames);
    }
}
