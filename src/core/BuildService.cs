using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Build.Common;
using Microsoft.TeamFoundation.Build.Workflow;
using Microsoft.TeamFoundation.Client;

namespace QBuild.Core
{
    public class BuildService : IBuildService
    {
        private readonly TfsConnection _connection;

        public BuildService(TfsConnection connection)
        {
            _connection = connection;
        }

        public IBuildRequest CreateRequest(BuildArguments arguments)
        {
            IBuildRequest request = new BuildRequest
            {
                ProjectCollection = arguments.Collection,
                ProjectName = arguments.ProjectName,
                BuildDefinition = arguments.BuildDefinition,
                BuildVersion = arguments.BuildVersion,
                PollingInterval = TimeSpan.FromSeconds(arguments.PollingInterval),
                Timeout = TimeSpan.FromMinutes(arguments.Timeout),
                Verbose = arguments.Verbose
            };

            foreach (var additionalArgument in arguments.AdditionalArguments)
                request.BuildArguments.Add(additionalArgument.Key, additionalArgument.Value);

            return request;
        }

        public BuildStatus Build(IBuildRequest request, Action<IBuildResponse> action)
        {
            ValidateRequest(request);

            var buildServer = _connection.GetService<IBuildServer>();
            var buildDefinition = buildServer.GetBuildDefinition(request.ProjectName, request.BuildDefinition);
            var buildRequest = CreateBuildRequest(request, buildDefinition);
            var queuedBuild = buildServer.QueueBuild(buildRequest);

            var lastModifiedDate = DateTime.MinValue;

            queuedBuild.StatusChanged += (sender, args) =>
            {
                queuedBuild.Build.RefreshAllDetails();

                var nodes = queuedBuild.Build.Information.GetNodesByTypes(
                    new[]
                    {
                        InformationTypes.ActivityTracking, 
                        InformationTypes.BuildMessage, 
                        InformationTypes.BuildError, 
                        InformationTypes.BuildWarning
                    }, true);

                var output = new StringBuilder();
                var errors = new List<string>();

                foreach (var node in nodes)
                {
                    if (node.LastModifiedDate <= lastModifiedDate) continue;

                    ParseOutput(output, errors, node);
                    lastModifiedDate = node.LastModifiedDate;
                }

                var response = new BuildResponse
                {
                    Status = queuedBuild.Status, 
                    Output = output.ToString(), 
                    Errors = errors
                };

                action(response);
            };

            queuedBuild.WaitForBuildCompletion(request.PollingInterval, request.Timeout);

            return queuedBuild.Build.Status;
        }

        private static Microsoft.TeamFoundation.Build.Client.IBuildRequest CreateBuildRequest(IBuildRequest request, IBuildDefinition definition)
        {
            var buildRequest = definition.CreateBuildRequest();

            var parameters = WorkflowHelpers.DeserializeProcessParameters(buildRequest.ProcessParameters);

            if (request.Verbose)
                parameters[ProcessParameterMetadata.StandardParameterNames.Verbosity] = BuildVerbosity.Diagnostic;
            else
                parameters[ProcessParameterMetadata.StandardParameterNames.Verbosity] = BuildVerbosity.Detailed;

            foreach (var argument in request.BuildArguments)
                parameters[argument.Key] = argument.Value;

            buildRequest.ProcessParameters = WorkflowHelpers.SerializeProcessParameters(parameters);

            return buildRequest;
        }

        private static void ValidateRequest(IBuildRequest request)
        {
            if (request == null) 
                throw new ArgumentNullException("request");
            
            if (String.IsNullOrEmpty(request.ProjectCollection))
                throw new ArgumentException("Project Collection is required.");
            
            if (String.IsNullOrEmpty(request.ProjectName))
                throw new ArgumentException("Project Name is required.");

            if (String.IsNullOrEmpty(request.BuildDefinition))
                throw new ArgumentException("Build Definition is required.");

            if (String.IsNullOrEmpty(request.BuildVersion))
                throw new ArgumentException("Build Version is required.");
        }

        private static void ParseOutput(StringBuilder output, ICollection<string> errors, IBuildInformationNode buildInformation, int level = 0)
        {
            var identation = new String(' ', level);

            if (buildInformation.Fields.ContainsKey(InformationFields.DisplayText))
            {
                output.Append(identation).AppendLine(buildInformation.Fields[InformationFields.DisplayText]);

                if (buildInformation.Type == InformationTypes.BuildError)
                    errors.Add(buildInformation.Fields[InformationFields.DisplayText]);
            }

            if (buildInformation.Fields.ContainsKey(InformationFields.Message))
            {
                output.Append(identation).AppendLine(buildInformation.Fields[InformationFields.Message]);

                if (buildInformation.Type == InformationTypes.BuildError)
                    errors.Add(buildInformation.Fields[InformationFields.Message]);
            }

            if (buildInformation.Children.Nodes.Length <= 0)
                return;

            foreach (var node in buildInformation.Children.Nodes)
                ParseOutput(output, errors, node, level + 1);
        }
    }
}