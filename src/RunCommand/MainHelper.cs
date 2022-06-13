using System;
using System.Linq;
using System.Collections.Generic;

using YTS.Log;
using YTS.Git;
using System.Text;

namespace RunCommand
{
    public class MainHelper
    {
        private readonly Encoding encoding;
        private readonly ILog log;

        public MainHelper()
        {
            encoding = Encoding.UTF8;
            var logFile = ILogExtend.GetLogFilePath("MainHelper");
            log = new FilePrintLog(logFile, encoding);
        }

        public void OnExecute(bool isopenbash, string command)
        {
            throw new NotImplementedException();
        }
    }
}
