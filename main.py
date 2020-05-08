# coding: UTF-8

import io
import sys
import os
import re
import json
import copy
import argparse
import time

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

from YTSTools import font_format
from YTSTools import convert
from YTSTools import file

import workTree

def UserArgs():
    try:
        parser = argparse.ArgumentParser(description='Git检查遍历程序')
        parser.add_argument('--openbash', '-o',
                            help='是否需要自动打开 Git Bash 命令窗口', action='store_true')
        parser.add_argument('--command', '-c',
                            help='所有仓库需要执行的命令, 只在所有仓库都干净的时候执行', type=str)
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
        }, ]
    })

def main(userArgs):
    # 读取 json 配置文件
    config = develop_config()

    tasks = Get_ITask_List(userArgs)

    gits = workTree.GitRepos(config)
    repos = gits.scattered_repos()

    console = Console()
    console.WriteIntervalLine("Git Check Program Start Run:")

    not_clean_count = 0
    for repo in repos:
        repo_msgs = []
        for ITask in tasks:
            rrepo = ITask.OnExecute(repo)
            if rrepo:
                repo = rrepo
            msgs = ITask.PrintResult(repo)
            if msgs:
                printContent = '\n'.join(msgs)
                repo_msgs.append(printContent)

        if not repo_is_clean(repo):
            not_clean_count += 1
            console.WriteIntervalLine('\n'.join(repo_msgs))
        else:
            console.Write('\n'.join(repo_msgs))

    if not_clean_count <= 0:
        resultMsg = font_format.font_green("All warehouses are very clean... ok!")
    else:
        resultMsg = "Need Oper Repo Count: {}".format(font_format.font_red(not_clean_count))
    console.WriteIntervalLine(resultMsg)

def repo_is_clean(repo):
    git = repo.get('git', {})
    git_status = git.get('status', {})
    git_status_is_clean = git_status.get('is_clean', None)
    return git_status_is_clean

class Console():
    def __init__(self):
        self.beforeIsIntervalLine = False

    def Write(self, strContent):
        sys.stdout.write(strContent + "\n")
        sys.stdout.flush()
        self.beforeIsIntervalLine = False

    def WriteIntervalLine(self, strContent):
        if not self.beforeIsIntervalLine:
            self.Write(font_format.interval_line())
        self.Write(strContent)
        self.Write(font_format.interval_line())
        self.beforeIsIntervalLine = True

if __name__ == '__main__':
    userArgs = UserArgs()
    if userArgs:
        main(userArgs)
