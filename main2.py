import sys
import os
import re
import json

import file
import convert
import font_format

def main(config):
    print(font_format.interval_line())
    print("Start find need git operating repositories:")

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

        check_repo_status = git_status(son_paths);
        if not check_repo_status.execute_results():
            print(font_format.interval_line())
            print("root: {}".format(root_path));
        for msg in check_repo_status.results:
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

def execute_command(command_string):
    with os.popen(r'git status', 'r') as f:
        text = f.read()
    return text

class git_status():
    def __init__(self, git_project_paths):
        self.git_project_paths = git_project_paths
        self.results = []
    def execute_results(self):
        is_have_error_git_repo = False
        for path in self.git_project_paths:
            is_exception, rcode, message_git_status = self.check_git_project(path)
            if is_exception:
                self.show_git_repo_result(path, rcode, message_git_status)
                is_have_error_git_repo = True
        if not is_have_error_git_repo:
            self.results.append(font_format.interval_line())
            self.results.append(font_format.font_green("All warehouses are very clean... ok!"))
            self.results.append(font_format.interval_line())
            return False
        return True

    def check_git_project(self, path):
        os.chdir(path)
        message_git_status = execute_command('git status')
        ris, rcode = self.is_need_hint_status_message(message_git_status)
        if ris and rcode != "":
            return True, rcode, message_git_status
        return False, '', ''

    def is_need_hint_status_message(self, msg):
        yes = [
            "Changes not staged for commit:",
            "Changes to be committed:",
            "(use \"git push\" to publish your local commits)",
            "Untracked files:"
        ]
        for code in yes:
            if code in msg:
                return True, code
        no = [
            "nothing to commit, working tree clean"
        ]
        for code in no:
            if code in msg:
                return False, code
        return False, ""

    def show_git_repo_result(self, path, rcode, message_git_status):
        if convert.is_window_system() and convert.is_window_path(path):
            path = convert.to_linux_path(path)

        rcode_format = font_format.font_fuchsia(rcode)
        rcode_format = message_git_status.replace(rcode, rcode_format)
        rcode_format = rcode_format.strip('\n')

        red_url = path.strip('\n')
        red_url = font_format.font_red(red_url)

        self.results.append(font_format.interval_line())
        self.results.append("path: {}".format(red_url))
        self.results.append("out:\n{}".format(rcode_format))
        self.results.append(font_format.interval_line())

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    main(config)
