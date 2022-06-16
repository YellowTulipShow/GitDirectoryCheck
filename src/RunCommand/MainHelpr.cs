using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using YTS.Log;

namespace RunCommand
{
    public class MainHelpr
    {
        private readonly ILog log;
        private readonly Encoding encoding;

        public MainHelpr(ILog log)
        {
            this.log = log;
            encoding = Encoding.UTF8;
        }

        public void OnExecute(bool isOpenShell, string command)
        {
            throw new NotImplementedException();
        }
    }
}
