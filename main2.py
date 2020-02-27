# coding: UTF-8

import sys
import os
import re
import json

import file
import workTree

def main(config):
    git_project_paths = []
    global_ignores = config.get('ignores', [])
    roots = config.get('roots', [])
    for root in roots:
        root_path = root.get('path', None)
        if not root_path:
            continue
        ignores = root.get('ignores', [])
        ignores.extend(global_ignores)
        son_paths = subproject_address_list(root_path, ignores=ignores)
        git_project_paths.extend(son_paths)

    cwt = workTree.CheckWorkTree(git_project_paths);
    cwt.execute_results()
    for msg in cwt.results:
        print(msg)

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


if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    main(config)
