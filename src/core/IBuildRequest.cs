using System;
using System.Collections.Generic;

namespace QBuild.Core
{
    public interface IBuildRequest
    {
        string ProjectCollection { get; }
        string ProjectName { get; set; }
        string BuildDefinition { get; }
        string BuildVersion { get; }
        TimeSpan PollingInterval { get; }
        TimeSpan Timeout { get; }
        bool Verbose { get; }
        Dictionary<string, string> BuildArguments { get; }
    }
}