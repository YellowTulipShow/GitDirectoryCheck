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

def main(repos):
    for repo in repos:
        print(repo['linux_path'], repo['git'])

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    gits = workTree.GitRepos(config)
    repos = gits.scattered_repos();
    is_all_clean = gits.repos_check_status(repos)

    if not is_all_clean:
        print('有仓库需要手动操作, 后续批量任务无法执行!')
    else:
        print('所有仓库干净, 可以执行后续任务!')
        main(repos)

