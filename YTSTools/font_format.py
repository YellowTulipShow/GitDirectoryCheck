# coding:utf-8

def font_black(str_text):
    return "\033[1;30m{}\033[0m".format(str_text)

def font_red(str_text):
    # http://www.cnblogs.com/ping-y/p/5897018.html
    return "\033[1;31m{}\033[0m".format(str_text)

def font_green(str_text):
    return "\033[1;32m{}\033[0m".format(str_text)

def font_yellow(str_text):
    return "\033[1;33m{}\033[0m".format(str_text)

def font_blue(str_text):
    return "\033[1;34m{}\033[0m".format(str_text)

def font_fuchsia(str_text):
    return "\033[1;35m{}\033[0m".format(str_text)

def font_cyan(str_text):
    return "\033[1;36m{}\033[0m".format(str_text)

def font_white(str_text):
    return "\033[1;37m{}\033[0m".format(str_text)

def interval_line():
    return "\n{}\n".format("-" * 80)
