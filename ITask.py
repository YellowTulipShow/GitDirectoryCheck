# coding: UTF-8

# interface接口: 任务接口
class ITask():
    def __init__(self):
        pass

    """
    执行相关的任务操作
    Returns:
        repo 回传传入的仓库相关对象
    """
    def OnExecute(self, repo):
        return repo

    """
    输出打印结果
    Returns:
        string[] 打印结果
    """
    def PrintResult(self, repo):
        return []
