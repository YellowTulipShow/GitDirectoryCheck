# coding: UTF-8

import os

import convert
import font_format

class CheckWorkTree():
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
