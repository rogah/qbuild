using System.Collections.Generic;
using Microsoft.TeamFoundation.Build.Client;

namespace QBuild.Core
{
    public interface IBuildResponse
    {
        QueueStatus Status { get; set; }
        string Output { get; set; }
        ICollection<string> Errors { get; set; }
    }
}