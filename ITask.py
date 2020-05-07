# coding: UTF-8

# interface接口: 任务接口
class ITask():
    def __init__(self):

    """
    注入用户参数内容
    Returns:
        ArgumentParser 回传传入的解析器对象
    """
    def UserArgumentAdd(self, keyname, parser):
        return parser

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
