import sys
import os
import re
import json
import platform
from subprocess import Popen, PIPE

def read_config_json_file():
    t = os.path.split(sys.argv[0])
    try:
        cp = os.path.join(t[0], "config.json")
        f = open(cp, encoding='utf-8')
        return json.load(f);
    except Exception as e:
        c = {
            "def_check_paths": [
                t[0]
            ],
            "ignore_paths": []
        }
        print("c:", c)
        return c

# 读取 json 配置文件
c = read_config_json_file();

def is_window_path(path):
    return re.match(r"[a-zA-Z]:\\.*", path) != None

def to_linux_path(window_path):
    m = re.compile(r"([a-zA-Z]):\\(.*)", re.I | re.M | re.U)
    r = m.findall(window_path)[0]
    drive_letter = r[0]
    son_path = re.sub(r"\\+", "/", str(r[1]))
    return "/{}/{}".format(drive_letter, son_path)

def get_root_paths():
    roots = c["def_check_paths"]

    if len(roots) <= 0:
        roots.append(os.getcwd())

    if len(sys.argv) > 1:
        arg_path = sys.argv[1]
        roots = [arg_path];

    return roots;

def is_ignore_path(root):
    for ignore in c["ignore_paths"]:
        if ignore in root:
            return True
    return False;

def exe_command(str_command):
    # http://www.cnblogs.com/nerrissa/articles/5784746.html
    p = Popen(str_command, shell=True, stdout=PIPE, stderr=PIPE)
    p.wait()
    out = p.stdout.read();
    j = {
        "out": out.decode("utf-8"),
    }
    return j

def get_repositories_status_info(root):
    os.chdir(root)
    path = root
    if platform.system() == "Windows" and is_window_path(path):
        path = to_linux_path(path)
    info = {
        "path": path,
        "out": exe_command("git status")["out"],
    }
    return info

def get_all_git_repositories(root):
    rep_infos = []
    if os.path.isfile(root):
        return rep_infos

    if not os.path.isdir(root):
        raise TypeError("Root path not isfile and not isdir, ERROR!")

    if ".git" in os.listdir(root):
        info = get_repositories_status_info(root)
        rep_infos.append(info)
        return rep_infos

    # 当前路径不是 git 存储库
    folders = os.listdir(root)
    for folder in folders:
        path = os.path.join(root, folder)

        if is_ignore_path(path):
            continue

        silist = get_all_git_repositories(path)
        if len(silist) > 0:
            for i in silist:
                rep_infos.append(i)

    return rep_infos

def filter_infos(infos):
    status_codes = [
        "Changes not staged for commit:",
        "Changes to be committed:",
        "(use \"git push\" to publish your local commits)",
        "Untracked files:"
    ]

    rinfos = []
    for info in infos:
        for code in status_codes:
            msg = info["out"]
            if code in msg:
                color_code = font_fuchsia(code)
                info["out"] = msg.replace(code, color_code)
                rinfos.append(info)
                break

    return rinfos

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

def IntervalLine():
    print("\n{}\n\n".format("-" * 80), end="")

def print_list_infos(root, infos):
    if len(need_show_info) <= 0:
        print("root: {}".format(root));
        print(font_green("All warehouses are very clean... ok!"))
        IntervalLine()
        return

    for info in infos:
        url = info["path"].strip("\n")
        red_url = font_red(url)
        msg = info["out"].strip("\n")
        print("path: {}".format(red_url))
        print("out:\n{}".format(msg))
        IntervalLine()

def print_exception(root, e):
    color_root = font_blue(root)
    color_e = font_blue(e)
    print("root: {}".format(color_root));
    print("e: {}".format(color_e));
    IntervalLine()


# ----------------------------------------------------------------------------------------------------------------------

print("Start find need git operating repositories:")
IntervalLine()

# 开始执行程序:
for root in get_root_paths():
    rep_infos = []

    try:
        # 获取目录下所有仓库信息
        rep_infos = get_all_git_repositories(root)

    except Exception as e:
        # 打印错误信息
        print_exception(root, e)

    else:
        # 筛选过滤需要展示的仓库信息
        need_show_info = filter_infos(rep_infos)

        # 打印仓库信息
        print_list_infos(root, need_show_info)
