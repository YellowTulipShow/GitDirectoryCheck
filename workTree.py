# coding: UTF-8

import sys
import os
import re
import json
import copy

import file
import convert
import font_format

class GitRepos(object):
    def __init__(self, config):
        ctemplate = self.config_template()
        self.global_config = convert.fill_template(ctemplate, config)
        self.global_ignores = config.get('ignores', [])
        self.global_roots = config.get('roots', [])

    def scattered_repos(self):
        git_repos = {}
        for root in self.global_roots:
            root_path = root.get('path', None)
            if not root_path:
                continue
            root_config = convert.fill_template(self.global_config, root)
            root_ignores = root_config.get('ignores', [])
            root_ignores.extend(self.global_ignores)
            son_paths = self.subproject_address_list(root_path, ignores=root_ignores)
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

    def config_template(self):
        return {
            'linux_path': '',
            'window_path': '',
            'is_independent': False,
            'is_open_git_bash': False,
            'ignores': [],
        }

    def subproject_address_list(self, root, ignores=[]):
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
            r = self.subproject_address_list(path, ignores=ignores)
            results.extend(r)
        return results

class CheckStatus():
    def __init__(self, repo):
        self.repo = repo
        self.is_clean = True
        self.problem_result = '',
        self.linux_path = repo.get('linux_path', None)
        self.window_path = repo.get('window_path')

    def execute(self):
        cd_path = self.linux_path
        if convert.is_window_system():
            cd_path = self.window_path
        os.chdir(cd_path)
        result_message = convert.execute_command('git status')
        is_clean, keyword = self.check_exception_status(result_message)
        self.is_clean = is_clean
        self.repo['git'] = {
            'status': {
                'is_clean': is_clean,
            }
        }
        if not is_clean:
            self.problem_result = self.format_exception_message(keyword, result_message)
            self.repo['git']['status']['keyword'] = keyword
            self.repo['git']['status']['message'] = result_message

    def check_exception_status(self, msg):
        yes = [
            "Changes not staged for commit:",
            "Changes to be committed:",
            "(use \"git push\" to publish your local commits)",
            "Untracked files:"
        ]
        for code in yes:
            if code in msg:
                return False, code
        no = [
            "nothing to commit, working tree clean"
        ]
        for code in no:
            if code in msg:
                return True, code
        return True, ""

    def format_exception_message(self, keyword, result_message):
        keyword_format = font_format.font_fuchsia(keyword)
        result_message = result_message.replace(keyword, keyword_format)
        result_message = result_message.strip('\n')
        return result_message
