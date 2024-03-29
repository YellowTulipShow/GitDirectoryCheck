﻿# 更新日志

## 正式版本: v2.1.1 发布

* [问题已修复]: 当 `Git` 存储库存储 `Init` 状态时, 无法查询到分支信息导致报错, 后续存储库就没法检查
* 增加单存储库处理时的各模块异常捕获与处理, 更好的展示异常信息
* 修改部分类的命名歧义问题
* 优化代码: 删除多余的引用
* 在配置文件中增加字段: 版本号, 当程序读取的版本号与当前电脑不一致时, 覆盖最近的配置结构

## 正式版本: v2.1.0 发布

* 实现打开 `Git Bash Shell` 功能
* 增加配置文件的配置项
* 优化配置文件读取时输出路径展示样式

## 正式版本: v2.0.2 发布

* 单独管理 `IPrint` 打印输出接口, 并移植到 `DotNetCore.YTS.Solution` 项目中去
* 优化参数调用的名称与含义

## 正式版本: v2.0.1 发布

更新原因:

* 使用语言 `Python` 的程序总不是那么得心应手, 使用了 `C#` `.Net Core 3.1` 平台重写了功能
* 执行速度也得到了提升, 使用调用方法也更方便了

## 正式版本: v1.3.0 发布

更新原因:

* 原先是将所有的仓库计算出结果再统一输出到屏幕上, 但会出现多个功能重复计算仓库地址/输出不直观/单个仓库报错后续仓库无法执行, 也没有输出的缺陷

更新内容:

1. 将各个功能通过接口规范化执行与输出
2. 调用定义功能工厂
3. 定义统一的输出打印方法
4. 输出时避免重复间隔行的问题

## 正式版本: v1.2.4 发布

更新内容:

1. 修改运行配置文件名称
2. 弃用内置配置文件模板转为程序内默认定义
3. 封装公用 YTSTools 工具类模块
4. 增加脚本参数定义设置 -h --help 展示使用帮助
5. 新功能: 实现批量执行自定义命令
