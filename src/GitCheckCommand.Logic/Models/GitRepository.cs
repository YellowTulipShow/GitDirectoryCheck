using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace GitCheckCommand.Logic.Models
{
    public struct GitRepository
    {
        public DirectoryInfo Path { get; set; }
        public bool? IsOpenShell { get; set; }
        public string BranchName { get; set; }
    }
}
