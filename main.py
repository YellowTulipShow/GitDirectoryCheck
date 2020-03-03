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
    is_all_clean = reposCheckStatus(repos)
    if not is_all_clean:
        print('有仓库需要手动操作, 后续批量任务无法执行!')
    else:
        print('所有仓库干净, 可以执行后续任务!')

def reposCheckStatus(repos):
    is_all_clean = True
    clean_paths = []
    problems = []
    for repo in repos:
        linux_path = repo.get('linux_path', '');
        cs = workTree.CheckStatus(repo);
        cs.execute()
        if not cs.is_clean:
            is_all_clean = False
            problem = "path: {}\nout:\n{}".format(
                font_format.font_red(linux_path),
                cs.problem_result)
            problems.append(problem)
        else:
            clean_path = font_format.font_yellow(linux_path)
            if convert.is_window_system():
                window_path = repo.get('window_path', '');
                window_path = font_format.font_blue(window_path)
                clean_path += ' | {}'.format(window_path)
            clean_paths.append(clean_path)
    # 打印信息
    results = []
    results.append("Start find need git operating repositories:")
    if len(clean_paths) > 0:
        results.append('\n'.join(clean_paths))
    if is_all_clean:
        results.append(font_format.font_green("All warehouses are very clean... ok!"))
    elif len(problems) > 0:
        results.extend(problems)
    # print(font_format.interval_line())
    print('\n{}\n'.format(font_format.interval_line()).join(results))
    print(font_format.interval_line())
    return is_all_clean

if __name__ == '__main__':
    # 读取 json 配置文件
    config = file.read_program_config_DevelopToRelease(
        release_file_name = '.config.release.json',
        develop_file_name = '.config.develop.json')
    gits = workTree.GitRepos(config)
    repos = gits.scattered_repos();
    main(repos)
