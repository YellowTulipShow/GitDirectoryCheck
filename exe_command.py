# coding:utf-8
import os

# popen返回文件对象，跟open操作一样
with os.popen(r'git status', 'r') as f:
    text = f.read()
print(text) # 打印cmd输出结果
