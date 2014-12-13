using System;
using System.Collections.Generic;

namespace QBuild.Core
{
    public class BuildRequest : IBuildRequest
    {
        public BuildRequest()
        {
            BuildArguments = new Dictionary<string, string>();
        }

        public string ProjectCollection { get; set; }
        public string ProjectName { get; set; }
        public string BuildDefinition { get; set; }
        public string BuildVersion { get; set; }
        public TimeSpan PollingInterval { get; set; }
        public TimeSpan Timeout { get; set; }
        public bool Verbose { get; set; }
        public Dictionary<string, string> BuildArguments { get; private set; }
    }
}