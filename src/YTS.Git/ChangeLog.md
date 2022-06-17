# YTS.Logic 基础逻辑类库

## 版本更新日志

### v0.1.0

* 添加: 基本的常用命令定义
	* `IGit`					接口: Git 操作工具
	* `SubCommands.IGitAdd`		添加到暂存库子命令
	* `SubCommands.IGitStatus`	查看存储库状态信息子命令
	* `SubCommands.IGitCommit`	提交到存储库子命令
	* `SubCommands.IGitPull`	拉取远程仓库数据子命令
	* `SubCommands.IGitPush`	推送本地到远程仓库子命令
	* `Repository`				存储库配置信息
	* `GitHelper`				执行命令实现类