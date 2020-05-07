# coding: UTF-8

import io
import sys
import os
import re
import json
import copy
import argparse

sys.stdout = io.TextIOWrapper(sys.stdout.buffer,encoding='utf-8')

from YTSTools import file
from YTSTools import convert
from YTSTools import font_format
import workTree

def UserArgs():
    try:
        parser = argparse.ArgumentParser(description='Git检查遍历程序')
        parser.add_argument('--openbash', '-o', help='是否需要自动打开 Git Bash 命令窗口', action='store_true')
        parser.add_argument('--command', '-c', help='所有仓库需要执行的命令, 只在所有仓库都干净的时候执行', type=str)
        args = parser.parse_args()
        return {
            'openbash': args.openbash,
            'command': args.command,
        }
    except Exception as e:
        print("Command Input Error: \n", e)
        return None

def Get_ITask_List(userArgs):
    tasks = []
    tasks.append(workTree.RepoCheckStatus())
    tasks.append(workTree.RepoOpenGitBash(userArgs.get('openbash', False)))
    tasks.append(workTree.RepoExecuteCommand(userArgs.get('command', None)))
    return tasks

def develop_config():
    isWin = convert.is_window_system()
    index = 0 if isWin else 1
    return file.read_program_config('config.json', {
        "is_open_git_bash": False,
        "ignores": [
            "wwwroot$",
        ],
        "roots": [{
            "path": [
                "D:\\Work",
                "/var/work"
                ][index],
            "ignores": [
                "YTS.Test$",
                "YTS.Learn$",
            ]
        },]
    })

def main(userArgs):
    # 读取 json 配置文件
    config = develop_config()

    tasks = Get_ITask_List(userArgs);

    gits = workTree.GitRepos(config)
    repos = gits.scattered_repos();

    for repo in repos:
        for ITask in tasks:
            rrepo = ITask.OnExecute(repo)
            if rrepo:
                repo = rrepo
            msgs = ITask.PrintResult(repo)
            if msgs:
                print('\n'.join(msgs))

if __name__ == '__main__':
    userArgs = UserArgs()
    if userArgs:
        main(userArgs)
