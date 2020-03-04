# coding: UTF-8

import io
import sys
import os
import re
import json
import copy

sys.stdout = io.TextIOWrapper(sys.stdout.buffer,encoding='utf-8')

import file
import convert
import font_format
import workTree

def main():
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    gits = workTree.GitRepos(config)
    repos = gits.scattered_repos();
    is_all_clean = gits.repos_check_status(repos)
    follow_action(is_all_clean, repos)

def follow_action(is_all_clean, repos):
    is_can_continue = open_git_bash(is_all_clean, repos)
    if not is_can_continue:
        return
    # ... 更多动作

def open_git_bash(is_all_clean, repos):
    if is_all_clean:
        return True

    filec = file.read_program_file_DevelopToRelease(
        release_file_name = 'open_window_git_bash.bat',
        develop_file_name = 'open_window_git_bash.develop.bat')
    if not filec:
        return True

    for repo in repos:
        is_clean = repo.get('is_clean', False)
        is_open_git_bash = repo.get('is_open_git_bash', False)
        if is_clean or not is_open_git_bash:
            continue
        window_path = repo.get('window_path', None)
        if convert.is_window_system() and window_path:
            program_dir = os.path.split(os.path.realpath(__file__))[0]
            os.chdir(program_dir)
            cmd = '"start open_window_git_bash.bat {}"'.format(window_path)
            os.system(cmd)

    return True

if __name__ == '__main__':
    main()
