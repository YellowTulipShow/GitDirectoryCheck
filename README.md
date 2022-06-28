# Git 仓库检查脚本

## 开发目的:

通过此脚本可以遍历指定目录下的所有 `Git` 仓库, 检查他们是否有新的更改, 或者没有同步到远程仓库的更改

因为在工作中生活中有很多可能同时在修改的项目/仓库/笔记的"项目", 所以需要一个这样的脚本在一天的工作结束后, 自动检查它们的状态

## 运行测试版本

* 程序语言: `C#`
* 运行平台: `.Net Core 3.1`
* 与之前版本相比基于可以跨平台安装执行

### 使用

首先确保计算机中含有 `.Net Core 3.1 +` 运行时环境, 如果没有请自行下载

随便找一个目录 `git clone` 拉取下程序源码

调用命令即可运行:

```shell
$ dotnet run --project ./src/GitCheckCommand/GitCheckCommand.csproj
```

### 方便使用

快速调用脚本发布安装, 使用 `Powershell` 脚本

```powershell
./shell/find_all_project.ps1
./shell/release.ps1
./shell/install_command_packages.ps1
```

执行完成, 无错误即可使用命令程序:

```powershell
gits
```

## 配置文件说明

第一次运行时就根据程序内置的配置内容自动生成运行时配置文件

如 `Window` 系统下路径: `C:\Users\Administrator\.command_gitcheck_config.json`

也可以手动指定配置路径, 使用别名调用即可, 具体参数请使用 `-h` 查看

内容参数注解如下:
```json
{
    // 全局配置项: 如果仓库不干净是否, 自动打开对应的 Git Bash 程序
    // 目前还没有完成, 正在寻找实现思路
    "IsOpenShell": false,

    // 全局配置项: 对应的 Git Bash 程序路径地址
    "OpenShellGitBashExePath": "C:\\Program Files\\Git\\git-bash.exe",

    // 全局配置项, 循环递归查找指定目录下所有仓库的路径隐藏项
    // 在查找过程中会将每一项与路径进行正则表达式的匹配, 如果匹配成功则跳过不计算在程序运行范围内
    "IgnoresRegexs": [],

    // 全局必备项, 执行需要查找那些路径
    "Roots": [
        {
            // 如: 查找 C盘 Work文件夹下所有的Git仓库
            "Path": "C:\\Work",

            // 没有明确指明其他配置则继承上面的全局配置项
            "IsOpenShell": null,
            // 指定了需要单独屏蔽某些路径的正则表达式
            "IgnoresRegexs": [
                "YTS.Test$",
                "YTS.Learn$"
            ]
        }
    ]
}
```

## 参考学习地址
* [RadoRado 的 git检查脚本, 功能基本实现 但与我的想法还有一些区别, 所以以此为基础进行代码的更改](https://github.com/RadoRado/Statuser)
* [python脚本执行CMD命令并返回结果](https://blog.csdn.net/xgh1951/article/details/85244272)
* [Python 命令行参数的3种传入方式](https://tendcode.com/article/python-shell/)
* [使用 python 执行 shell 命令的几种常用方式](https://tendcode.com/article/python-shell-cmd/)
* [python argparse用法总结](https://www.jianshu.com/p/fef2d215b91d)
* [使用 .NET 的 ANSI 着色控制台输出](https://www.itbaoku.cn/post/433410/ANSI-Coloring-Console-Output-with-.NET)
* [Linux Shell命令——命令别名 alias（含“永久生效”方法）](https://blog.csdn.net/u013894429/article/details/79908554)
* [window中的cmd中设置别名(alias)及设置快捷键打开cmd](https://blog.csdn.net/yiranzhiliposui/article/details/83116819)
* [为 Windows PowerShell 设置 alias （命令行命令别名）](https://blog.csdn.net/lei_qi/article/details/106592404)
