 # coding: UTF-8

import json
import re
import datetime
import platform
import sys
import os

import convert

def get_program_path():
    self_program_path = sys.argv[0]
    (self_program_dir, self_program_file) = os.path.split(self_program_path)
    return self_program_dir

def read_program_config(file_name, default_config_dict):
    self_program_dir = get_program_path();
    config_file_path = to_abs_path(self_program_dir, file_name)
    if not os.path.isfile(config_file_path):
        config_json_file_write(config_file_path, default_config_dict)
    return config_json_file_read(config_file_path)

def read_program_config_DevelopToRelease(release_file_name, develop_file_name):
    self_program_dir = get_program_path();
    develop_file_path = to_abs_path(self_program_dir, develop_file_name)
    develop_file_content = config_json_file_read(develop_file_path)
    return read_program_config(release_file_name, develop_file_content)

def read_program_file(file_name, default_file_content):
    self_program_dir = get_program_path();
    file_path = to_abs_path(self_program_dir, file_name)
    if not os.path.isfile(file_path):
        file_write(file_path, default_file_content)
    return file_read(file_path)

def read_program_file_DevelopToRelease(release_file_name, develop_file_name):
    self_program_dir = get_program_path();
    develop_file_path = to_abs_path(self_program_dir, develop_file_name)
    develop_file_content = file_read(develop_file_path)
    return read_program_file(release_file_name, develop_file_content)

def recursive_route(root, is_ignore=None):
    def default_is_ignore(folder):
        return True
    if not is_ignore:
        is_ignore = default_is_ignore
    file_paths = []
    if os.path.isfile(root):
        file_paths.append(root)
        return file_paths
    if not os.path.isdir(root):
        return file_paths
    folders = os.listdir(root)
    for folder in folders:
        path = os.path.join(root, folder)
        path = path.replace('\\', '/')
        if is_ignore(path):
            continue
        son_paths = recursive_route(path, is_ignore=is_ignore)
        file_paths.extend(son_paths)
    return file_paths

def to_abs_directory(relatice_directory):
    abs_path = os.path.abspath(relatice_directory)
    if not os.path.exists(abs_path):
        os.makedirs(abs_path)
    return abs_path

def to_abs_path(relatice_directory, file):
    abs_directory = to_abs_directory(relatice_directory)
    def to_abs_file(abs_directory, file, symbol):
        if symbol in abs_directory:
            abs_directory = convert.trimEnd(abs_directory, symbol=symbol)
            abs_directory = abs_directory + symbol + file
        return abs_directory
    if '\\' in abs_directory:
        return to_abs_file(abs_directory, file, '\\')
    if '/' in abs_directory:
        return to_abs_file(abs_directory, file, '/')
    if 'Window' in platform.platform():
        return to_abs_file(abs_directory, file, '\\')
    return abs_directory

def file_read(abs_path):
    f = open(abs_path, 'r', encoding='utf-8')
    content = f.read()
    f.close()
    return content

def file_write(abs_path, content):
    file_dir = os.path.split(abs_path)[0]
    to_abs_directory(file_dir)
    f = open(abs_path, 'w', encoding='utf-8')
    f.write(content)
    f.close()
    return abs_path

# 原文链接：https://blog.csdn.net/mouday/article/details/91047387
class DateEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, datetime.datetime):
            return obj.strftime('%Y-%m-%d %H:%M:%S')
        elif isinstance(obj, datetime.date):
            return obj.strftime("%Y-%m-%d")
        else:
            return json.JSONEncoder.default(self, obj)

def config_json_file_read(abs_path):
    content = file_read(abs_path)
    content = re.sub(r",(\s*[\}\]])", r"\1", content)
    return json.loads(content)

def config_json_file_write(abs_path, dict):
    content = json.dumps(dict, indent=4, ensure_ascii=False, cls=DateEncoder)
    return file_write(abs_path, content)
