# coding: UTF-8

import os
import re
import platform
import copy

def execute_command(command_string):
    with os.popen(command_string, 'r') as f:
        text = f.read()
    return text

def is_window_system():
    return platform.system() == "Windows"

def is_window_path(path):
    return re.match(r"[a-zA-Z]:\\.*", path) != None

def to_linux_path(window_path):
    m = re.compile(r"([a-zA-Z]):\\(.*)", re.I | re.M | re.U)
    r = m.findall(window_path)[0]
    drive_letter = r[0]
    son_path = re.sub(r"\\+", "/", str(r[1]))
    return "/{}/{}".format(drive_letter, son_path)

def trimStart(vstr, symbol=r'\s+'):
    vstr = str(vstr)
    vstr = re.sub(r"^" + symbol, "", vstr)
    return vstr

def trimEnd(vstr, symbol=r'\s+'):
    vstr = str(vstr)
    vstr = re.sub(symbol + r"$", "", vstr)
    return vstr

def trim(vstr, symbol=r'\s+'):
    vstr = trimStart(vstr, symbol=symbol)
    vstr = trimEnd(vstr, symbol=symbol)
    return vstr

def copy_dict(dict_old, dict_new):
    r = dict_old
    for key in dict_new:
        v = dict_new[key]
        if 'dict' in str(type(v)):
            ov = r.get(key, None)
            if ov:
                r[key] = copy_dict(ov, v)
            else:
                r[key] = copy_dict({}, v)
        else:
            r[key] = v
    return r

def fill_template(dict_template, dict_source):
    r = copy.deepcopy(dict_template)
    for key in r:
        sv = dict_source.get(key, None)
        if not sv:
            continue
        nv = copy.deepcopy(sv)
        rv = copy.deepcopy(r[key])
        if 'dict' in str(type(rv)):
            rv = copy_dict(rv, nv)
        elif 'list' in str(type(rv)):
            rv.extend(nv)
        else:
            rv = nv
        r[key] = rv
    return r
