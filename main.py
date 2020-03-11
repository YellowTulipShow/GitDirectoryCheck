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
        print(e)
        return None

def main(userArgs):
    # 读取 json 配置文件
    config = develop_config()
    gits = workTree.GitRepos(config)
    repos = gits.scattered_repos();
    is_all_clean = gits.repos_check_status(repos)
    follow_action(is_all_clean, repos, userArgs)

def develop_config():
    workaddress = '/var/work'
    if convert.is_window_system():
        workaddress = 'D:\\Work'
    return file.read_program_config('config.json', {
        "is_open_git_bash": False,
        "ignores": [
            "wwwroot$",
        ],
        "roots": [{
            "path": workaddress,
            "ignores": [
                "YTS.Test$",
                "YTS.Learn$",
            ]
        },]
    })

def follow_action(is_all_clean, repos, userArgs):
    is_can_continue = open_git_bash(is_all_clean, repos, userArgs.get('openbash', False))
    if not is_can_continue:
        return

    execute_command(is_all_clean, repos, userArgs)
    # ... 更多动作

def open_git_bash(is_all_clean, repos, is_user_openbash):
    if is_all_clean:
        return True

    filec = file.read_program_file_DevelopToRelease(
        release_file_name = 'open_window_git_bash.bat',
        develop_file_name = 'open_window_git_bash.develop.bat')
    if not filec:
        return True

    for repo in repos:
        is_clean = repo.get('git', {}).get('status', {}).get('is_clean', False)
        is_open_git_bash = repo.get('is_open_git_bash', False)
        is_open_git_bash = is_open_git_bash or is_user_openbash
        if not is_clean and is_open_git_bash:
            window_path = repo.get('window_path', None)
            if convert.is_window_system() and window_path:
                program_dir = os.path.split(os.path.realpath(__file__))[0]
                os.chdir(program_dir)
                cmd = '"start open_window_git_bash.bat {}"'.format(window_path)
                os.system(cmd)

    return True

def execute_command(is_all_clean, repos, userArgs):
    command = userArgs.get('command', None)
    if is_all_clean and command:
        results = []
        print(font_format.interval_line())
        results.append("批量执行命令:{}".format(command))
        for repo in repos:
            linux_path = repo.get('linux_path', '')
            window_path = repo.get('window_path', '')
            is_window = convert.is_window_system()
            cmd = workTree.Command(repo)
            rmsg = cmd.execute(command)
            rmsg = convert.trimEnd(rmsg, '\n')
            msgs = [ 'linux_path: {}'.format(font_format.font_red(linux_path)), ]
            if is_window:
                msgs.append('window_path: {}'.format(font_format.font_blue(window_path)))
            msgs.append('Message:\n{}'.format(rmsg))
            results.append('\n'.join(msgs))
        print('\n{}\n'.format(font_format.interval_line()).join(results))
        print(font_format.interval_line())

if __name__ == '__main__':
    userArgs = UserArgs()
    if userArgs:
        main(userArgs)
