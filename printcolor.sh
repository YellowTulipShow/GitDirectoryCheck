# /bin/bash


#字体颜色：30m-37m 黑、红、绿、黄、蓝、紫、青、白
str="kimbo zhang"
echo -e "\e[30m ${str}\e[0m"      ## 黑色字体
echo -e "\e[31m ${str}\e[0m"      ## 红色
echo -e "\e[32m ${str}\e[0m"      ## 绿色
echo -e "\e[33m ${str}\e[0m"      ## 黄色
echo -e "\e[34m ${str}\e[0m"      ## 蓝色
echo -e "\e[35m ${str}\e[0m"      ## 紫色
echo -e "\e[36m ${str}\e[0m"      ## 青色
echo -e "\e[37m ${str}\e[0m"      ## 白色

#背景颜色：40-47 黑、红、绿、黄、蓝、紫、青、白
str="kimbo zhang"
echo -e "\e[41;37m ${str} \e[0m"     ## 红色背景色，白色字体
echo -e "\e[41;33m ${str} \e[0m"     ## 红底黄字
echo -e "\e[1;41;33m ${str} \e[0m"   ## 红底黄字 高亮加粗显示
echo -e "\e[5;41;33m ${str} \e[0m"   ## 红底黄字 字体闪烁显示
echo -e "\e[47;30m ${str} \e[0m"     ## 白底黑字
echo -e "\e[40;37m ${str} \e[0m"     ## 黑底白字
