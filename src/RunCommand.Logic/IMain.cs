
using RunCommand.Logic.Models;

namespace RunCommand.Logic
{
    /// <summary>
    /// 接口: 主逻辑执行
    /// </summary>
    public interface IMain
    {
        /// <summary>
        /// 执行 Git 检查
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        /// <param name="commandOptions">命令选项</param>
        void OnExecute(string configFilePath, CommandOptions commandOptions);
    }
}
