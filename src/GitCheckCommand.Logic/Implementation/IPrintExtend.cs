using System;

using GitCheckCommand.Logic.Models;

namespace GitCheckCommand.Logic
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

        private readonly static string interval_line = $"\n{"".PadLeft(80, '-')}\n";
        private static bool isBeforeWriteIntervalLine = false;
        /// <summary>
        /// д������
        /// </summary>
        /// <param name="print">����ӿ�</param>
        public static void WriteIntervalLine(this IPrint print)
        {
            print.WriteLine(interval_line);
        }
    }
}
