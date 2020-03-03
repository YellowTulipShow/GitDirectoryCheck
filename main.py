# coding: UTF-8

import sys
import os
import re
import json
import copy

import file
import convert
import workTree

def main():
    pass

def git_project_deal_with(git_repos, config):
    cwt = workTree.CheckWorkTree(git_repos, config=config);
    cwt.execute_results()
    for msg in cwt.results:
        print(msg)

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    git_repos = workTree.GitRepos(config)
    results = git_repos.analyze();
    print(results)
