# coding: UTF-8

import sys
import os
import re
import json
import copy

import file
import convert
import workTree

def analyze_git_repos(config):
    ctemplate = git_repo_config_template()
    global_config = convert.fill_template(ctemplate, config)
    global_ignores = config.get('ignores', [])
    global_roots = config.get('roots', [])
    git_repos = {}
    for root in global_roots:
        root_path = root.get('path', None)
        if not root_path:
            continue
        root_config = convert.fill_template(global_config, root)
        root_ignores = root_config.get('ignores', [])
        root_ignores.extend(global_ignores)
        son_paths = subproject_address_list(root_path, ignores=root_ignores)
        for path in son_paths:
            repo_config = copy.deepcopy(root_config)
            if convert.is_window_path(path):
                repo_config['linux_path'] = convert.to_linux_path(path)
                repo_config['window_path'] = path
            else:
                repo_config['linux_path'] = path
                repo_config['window_path'] = ''
            repo = git_repos.get(path, None)
            if repo is None:
                git_repos[path] = repo_config
            else:
                git_repos[path] = convert.copy_dict(repo, repo_config)
    return [git_repos[repo] for repo in git_repos]

def git_repo_config_template():
    return {
        'linux_path': '',
        'window_path': '',
        'is_independent': False,
        'is_open_git_bash': False,
        'git': {
            'status': {
                'is_clean': False,
                'message': '',
            },
        },
    }

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
    repos = analyze_git_repos(config)
    print(repos)
