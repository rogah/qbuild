using System;
using Microsoft.TeamFoundation.Build.Client;

namespace QBuild.Core
{
    public interface IBuildService
    {
        IBuildRequest CreateRequest(BuildArguments arguments);
        BuildStatus Build(IBuildRequest request, Action<IBuildResponse> action);
    }
}