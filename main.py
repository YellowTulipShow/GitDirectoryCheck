# coding: UTF-8

import sys
import os
import re
import json

import file
import workTree

def main(config):
    global_is_independent_root_execute = config.get('is_independent_root_execute', False)
    global_is_open_git_bash = config.get('is_open_git_bash', False)
    global_ignores = config.get('ignores', [])
    global_roots = config.get('roots', [])

    git_project_paths = []
    for root in global_roots:
        root_path = root.get('path', None)
        if not root_path:
            continue
        is_independent_root_execute = root.get('is_independent_root_execute', False)
        is_independent_root_execute = is_independent_root_execute or global_is_independent_root_execute
        is_open_git_bash = root.get('is_open_git_bash', False)
        is_open_git_bash = is_open_git_bash or global_is_open_git_bash
        ignores = root.get('ignores', [])
        ignores.extend(global_ignores)

        son_paths = subproject_address_list(root_path, ignores=ignores)
        git_project_paths.extend(son_paths)

        if is_independent_root_execute:
            git_project_deal_with(son_paths, config={
                'is_open_git_bash': is_open_git_bash
            })

    if not global_is_independent_root_execute:
        git_project_deal_with(git_project_paths, config={
            'is_open_git_bash': global_is_open_git_bash
        })

def subproject_address_list(root, ignores=[]):
    def is_ignore(folder):
        if ignores == None or len(ignores) <= 0:
            return False
        for ig in ignores:
            if re.search(ig, folder, re.M|re.I):
                return True
        return False
    if os.path.isfile(root):
        return []
    if not os.path.isdir(root):
        return []
    os.chdir(root)
    folders = os.listdir(root)
    if '.git' in folders:
        return [root]
    results = []
    for folder in folders:
        path = os.path.join(root, folder)
        if is_ignore(path):
            continue
        r = subproject_address_list(path, ignores=ignores)
        results.extend(r)
    return results

def git_project_deal_with(git_project_paths, config):
    cwt = workTree.CheckWorkTree(git_project_paths, config=config);
    cwt.execute_results()
    for msg in cwt.results:
        print(msg)

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    main(config)
