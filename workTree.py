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

    def analyze(self):
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
            'git': {
                'status': {
                    'is_clean': False,
                    'message': '',
                },
            },
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
    def __init__(self, git_project_paths, config):
        self.git_project_paths = git_project_paths
        self.config = config
        self.results = []
        self.is_clean = True

    def execute_results(self):
        self.output_check_start_message();
        self.is_clean = True

        for path in self.git_project_paths:
            os.chdir(path)
            result_message = convert.execute_command('git status')
            is_clean, keyword = self.check_exception_status(result_message)
            if is_clean:
                continue
            self.is_clean = False
            self.output_exception_message(path, keyword, result_message)

        if self.is_clean:
            self.output_all_clean_message();

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

    def output_check_start_message(self):
        self.results.append(font_format.interval_line())
        self.results.append("Start find need git operating repositories:")

    def output_exception_message(self, path, keyword, result_message):
        path_format = path
        if convert.is_window_system() and convert.is_window_path(path):
            path_format = convert.to_linux_path(path)
            if self.config.get('is_open_git_bash', False):
                this_path = os.path.split(os.path.realpath(__file__))[0]
                os.system('"' + this_path + '\\open_git_bash.bat ' + path + '"')
        path_format = path_format.strip('\n')
        path_format = font_format.font_red(path_format)

        keyword_format = font_format.font_fuchsia(keyword)
        result_message = result_message.replace(keyword, keyword_format)
        result_message = result_message.strip('\n')

        self.results.append(font_format.interval_line())
        self.results.append("path: {}".format(path_format))
        self.results.append("out:\n{}".format(result_message))
        self.results.append(font_format.interval_line())

    def output_all_clean_message(self):
        self.results.append(font_format.interval_line())
        for path in self.git_project_paths:
            if convert.is_window_path(path):
                linux_path = convert.to_linux_path(path)
                linux_path = font_format.font_yellow(linux_path)
                window_path = font_format.font_blue(path)
                path_format = '{}{}{}'.format(linux_path, ' | ', window_path)
            else:
                linux_path = font_format.font_yellow(path)
                path_format = '\t{}'.format(linux_path)
            self.results.append(path_format)
        self.results.append(font_format.interval_line())
        self.results.append(font_format.font_green("All warehouses are very clean... ok!"))
        self.results.append(font_format.interval_line())
