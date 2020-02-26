import sys
import os
import re
import json

import file
import convert
import font_format

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

    is_have_error_git_repo = False
    for path in git_project_paths:
        if check_git_project(path):
            is_have_error_git_repo = True
    if not is_have_error_git_repo:
        # print("root: {}".format(root));
        print(font_format.font_green("All warehouses are very clean... ok!"))
        print(font_format.interval_line())

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
    os.chdir(path)
    message_git_status = execute_command('git status')
    ris, rcode = is_need_hint_status_message(message_git_status)
    if ris and rcode != "":
        show_git_repo_result(path, rcode, message_git_status)
        return True
    return False

def execute_command(command_string):
    with os.popen(r'git status', 'r') as f:
        text = f.read()
    return text

def is_need_hint_status_message(msg):
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

def show_git_repo_result(path, rcode, message_git_status):
    if convert.is_window_system() and convert.is_window_path(path):
        path = convert.to_linux_path(path)

    rcode_format = font_format.font_fuchsia(rcode)
    rcode_format = message_git_status.replace(rcode, rcode_format)
    rcode_format = rcode_format.strip('\n')

    red_url = path.strip('\n')
    red_url = font_format.font_red(red_url)

    print(font_format.interval_line())
    print("path: {}".format(red_url))
    print("out:\n{}".format(rcode_format))
    print(font_format.interval_line())

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    main(config)
