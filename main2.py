import sys
import os
import re
import json

import file

from git import Repo

def main(config):
    git_project_paths = []
    global_ignores = config.get('ignores', [])
    for root in config.get('roots', []):
        root_path = root.get('path', None)
        if not root_path:
            continue
        ignores = root.get('ignores', [])
        ignores.extend(global_ignores)
        son_paths = subproject_address_list(root_path, ignores=ignores)
        git_project_paths.extend(son_paths)

    for path in git_project_paths:
        check_git_project(path)
        break

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

def check_git_project(path):
    pass
    # repo = Repo(path)
    # branch_name = repo.head.reference
    # if repo.is_dirty():
    #     print('repo.untracked_files:', repo.untracked_files)
    # print(r)

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    main(config)
