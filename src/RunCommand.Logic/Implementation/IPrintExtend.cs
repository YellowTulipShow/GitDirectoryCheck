using System;

using RunCommand.Logic.Models;

namespace RunCommand.Logic
{
    /// <summary>
    /// ��̬��չ�ӿ�: ��ӡ����ӿ�
    /// </summary>
    public static class IPrintExtend
    {
        /// <summary>
        /// Ĭ�Ϻڵ�, д������
        /// </summary>
        /// <param name="printColor">��ӡ����ӿں�����ɫ��</param>
        /// <param name="content">��Ϣ����</param>
        /// <param name="textColor">�ı�������ɫ</param>
        public static void Write(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.Write(content, textColor, EPrintColor.None);
        }

        /// <summary>
        /// Ĭ�Ϻڵ�, д��һ������
        /// </summary>
        /// <param name="printColor">��ӡ����ӿں�����ɫ��</param>
        /// <param name="content">��Ϣ����</param>
        /// <param name="textColor">�ı�������ɫ</param>
        public static void WriteLine(this IPrintColor printColor, string content, EPrintColor textColor)
        {
            printColor.WriteLine(content, textColor, EPrintColor.None);
        }
    }
}
