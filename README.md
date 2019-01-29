# Git 仓库检查脚本

通过此脚本可以遍历 指定目录下的所有 `Git` 仓库, 检查他们是否有新的更改, 或者没有同步到远程仓库的更改

因为在工作中生活中有很多可能同时再修改的 项目/仓库/笔记 之类的, 所以需要一个这样的脚本来再一天的工作结束后, 自动检查它们的状态

这里使用 `Python` 脚本再执行这些工作, 开发与测试使用的版本是 `Python 3.6.2`, 其他版本请自行测试

## 使用:
```shell
$ python main.py [path]
```

`path` 参数可选, 如果不指定就采用 相对路径的 `./config.json` 内容路径进行检查

`config.json` 文件格式如下:
```json
{
    "def_check_paths": [
        "D:\\ZRQWork\\",
        "D:\\Work",
    ],
    "ignore_paths": [
        "DevelopmentTools",
        "JianGuoYunFolder",
        ".metadata",
    ]
}
```

`def_check_paths` 参数为: 默认的检查路径, 已数组形式写入多个

`ignore_paths` 参数为: 遍历递归循环过程中, 子路径中如果出现这些字符串就忽略执行, 同时注意 `window` 路径的间隔平台执行 路径使用 `window` 格式

## 方便使用
在 `shell` 中添加一个命令别名, 以便于使用, 在 `window` 平台下建议直接使用 `git bash` 命令行使用

再 `~/.bashrc` 文件加入一行:
```shell
alias gitdc='python /D/ZRQWork/YTS.ZRQ/PythonScripts/git_directory_check/main.py'
```

之后再 `shell` 中就可以直接使用命令了

```shell
$ gitdc
```


## 参考学习地址
```shell
# RadoRado 的 git检查脚本, 功能基本实现
# 但与我的想法还有一些区别, 所以以此为基础进行代码的更改
https://github.com/RadoRado/Statuser
```
