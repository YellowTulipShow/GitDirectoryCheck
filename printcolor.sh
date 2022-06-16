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


echo "-------------------------------------------------"

# 定义颜色变量
RED='\e[1;31m' # 红
GREEN='\e[1;32m' # 绿
YELLOW='\e[1;33m' # 黄
BLUE='\e[1;34m' # 蓝
PINK='\e[1;35m' # 粉红
RES='\e[0m' # 清除颜色
​
echo -e "${RED}Red${RES}"
echo -e "${GREEN}Green${RES}"
echo -e "${YELLOW}Yellow${RES}"
echo -e "${BLUE}Blue${RES}"
echo -e "${PINK}Pink${RES}"
echo
echo -e "\e[30m 黑色 \e[0m"
echo -e "\e[31m 红色 \e[1m"
echo -e "\e[31m 红色 \e[0m"
echo -e "\e[32m 绿色 \e[0m"
echo -e "\e[33m 黄色 \e[1m"
echo -e "\e[33m 黄色 \e[0m"
echo -e "\e[34m 蓝色 \e[0m"
echo -e "\e[35m 紫色 \e[0m"
echo -e "\e[36m 青色 \e[0m"
echo -e "\e[37m 白色 \e[0m"
echo
echo -e "\e[40m 黑底 \e[0m"
echo -e "\e[41m 红底 \e[0m"
echo -e "\e[42m 绿底 \e[0m"
echo -e "\e[43m 黄底 \e[0m"
echo -e "\e[44m 蓝底 \e[0m"
echo -e "\e[45m 紫底 \e[0m"
echo -e "\e[46m 青底 \e[0m"
echo -e "\e[47m 白底 \e[0m"
echo
echo -e "\e[90m 黑底黑字 \e[0m"
echo -e "\e[91m 黑底红字 \e[0m"
echo -e "\e[92m 黑底绿字 \e[0m"
echo -e "\e[93m 黑底黄字 \e[0m"
echo -e "\e[94m 黑底蓝字 \e[0m"
echo -e "\e[95m 黑底紫字 \e[0m"
echo -e "\e[96m 黑底青字 \e[0m"
echo -e "\e[97m 黑底白字 \e[0m"