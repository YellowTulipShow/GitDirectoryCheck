using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace GitCheckCommand.Logic.Models
{
    public struct GitRepositoryStatus
    {
        public bool IClean { get; set; }
        public string[] StatusMsgs { get; set; }
        public int NoCleanMsgIndex { get; set; }
    }
}
