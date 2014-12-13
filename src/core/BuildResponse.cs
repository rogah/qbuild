using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.Client;

namespace QBuild.Core
{
    public class BuildResponse : IBuildResponse
    {
        public QueueStatus Status { get; set; }
        public string Output { get; set; }
        public ICollection<string> Errors { get; set; }
    }
}