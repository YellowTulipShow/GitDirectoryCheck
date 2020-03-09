# Git 仓库检查脚本

## 开发目的:

通过此脚本可以遍历指定目录下的所有 `Git` 仓库, 检查他们是否有新的更改, 或者没有同步到远程仓库的更改

因为在工作中生活中有很多可能同时在修改的项目/仓库/笔记的"项目", 所以需要一个这样的脚本在一天的工作结束后, 自动检查它们的状态

## 运行测试版本

* 程序语言: `Python`
* 版本: `Python 3.6.0+`
* `Window`下运行建议使用Git自带的 `Git Bash` 命令行程序
* `Linux`下使用`Shell`即可

### 使用

随便找一个目录 `git clone` 拉取下程序源码

调用命令即可运行:

```shell
$ python main.py
```

### 方便使用

在 `shell` 中添加一个命令别名, 以便于使用

再 `~/.bashrc` 文件加入一行:
```shell
alias gits='python /(linux格式的路径)/GitDirectoryCheck/main.py'
```

之后再 `shell` 中就可以直接使用命令了

```shell
$ gits
```

## 配置文件说明

程序根目录下有默认的配置文件: `.config.develop.json`

再第一次运行`main.py`时就根据`.config.develop.json`的内容自动生成运行时配置文件: `.config.release.json`

随意修改`.config.release.json`的内容, 达到配置程序的目的

内容参数注解如下:
```json
{
    // 全局配置项: 如果仓库不干净是否, 自动打开对应的Git Bash程序
    // 目前仅在 Window系统下有效, 同时注意修改脚本文件 open_window_git_bash.bat: Git Bash程序的安装路径
    "is_open_git_bash": false,

    // 全局配置项, 循环递归查找指定目录下所有仓库的路径隐藏项
    // 在查找过程中会将每一项与路径进行正则表达式的匹配, 如果匹配成功则跳过不计算在程序运行范围内
    "ignores": [
        "wwwroot$",
    ],

    // 全局必备项, 执行需要查找那些路径
    "roots": [
        {
            // 如: 查找C盘Work文件夹下所有的Git仓库
            "path": "C:\\Work",
            // 没有明确指明其他配置则继承上面的全局配置项
        },
        {
            // 如: 查找D盘Work文件夹下所有的Git仓库
            "path": "D:\\Work",
            // 指定了需要单独屏蔽某些路径的项
            "ignores": [
                "YTS.Test$",
                "YTS.Learn$",
            ],
        },
        {
            // 为此仓库单独配置
            "path": "D:\\Work\\YTS.ZRQ\\GitDirectoryCheck",
            // 指定了自动打开Git Bash程序的配置
            "is_open_git_bash": true,
        },
    ]
}

```

## 参考学习地址
* [RadoRado 的 git检查脚本, 功能基本实现 但与我的想法还有一些区别, 所以以此为基础进行代码的更改](https://github.com/RadoRado/Statuser)
* [python脚本执行CMD命令并返回结果](https://blog.csdn.net/xgh1951/article/details/85244272)
* [Python 命令行参数的3种传入方式](https://tendcode.com/article/python-shell/)
* [使用 python 执行 shell 命令的几种常用方式](https://tendcode.com/article/python-shell-cmd/)
* [python argparse用法总结](https://www.jianshu.com/p/fef2d215b91d)
