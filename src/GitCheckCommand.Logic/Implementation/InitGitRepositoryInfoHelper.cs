using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using GitCheckCommand.Logic.Models;

using YTS.Log;

namespace GitCheckCommand.Logic.Implementation
{
    public class InitGitRepositoryInfoHelper
    {
        private readonly ILog log;
        private readonly CommandOptions commandOptions;

        public InitGitRepositoryInfoHelper(ILog log, CommandOptions commandOptions)
        {
            this.log = log;
            this.commandOptions = commandOptions;
        }

        public GitRepository OnExecute(GitRepository repository)
        {
            return repository;
        }
    }
}
