using System;
using Fclp;

namespace QBuild.Core
{
    public class BuildArgumentParser : IArgumentParser
    {
        private readonly IFluentCommandLineParser<BuildArguments> _builder;

        public BuildArgumentParser()
        {
            _builder = new FluentCommandLineParser<BuildArguments>();

            _builder.Setup(arg => arg.Collection)
                .As('c', "collection")
                .WithDescription("Collection url (e.g. https://tsf.dell.com:8080/tfs/dfs).")
                .Required();

            _builder.Setup(arg => arg.ProjectName)
                .As('p', "project")
                .WithDescription("Project name (e.g. Project-Name).")
                .Required();

            _builder.Setup(arg => arg.BuildDefinition)
                .As('d', "definition")
                .WithDescription("Build definition name (e.g. ProjectName-Relese#).")
                .Required();

            _builder.Setup(arg => arg.BuildVersion)
                .As('v', "version")
                .WithDescription("Build version (e.g. ProjectName-20140110.1).")
                .Required();

            _builder.Setup(arg => arg.PollingInterval)
                .As('i', "interval")
                .WithDescription("Polling interval in seconds. Default is 5.")
                .SetDefault(BuildArguments.DefaultPollingInterval);

            _builder.Setup(arg => arg.Timeout)
                .As('t', "timeout")
                .WithDescription("Timeout in minutes. Default is 30.")
                .SetDefault(BuildArguments.DefaultTimeout);

            _builder.Setup(arg => arg.Verbose)
                .As('x', "verbose")
                .WithDescription("Verbose logs. Default is false.")
                .SetDefault(false);
        }

        public IParseResult Parse(string[] arguments)
        {
            var result = _builder.Parse(arguments);

            var buildArguments = _builder.Object;
            buildArguments.AdditionalArguments = result.AdditionalOptionsFound;

            return new ParseResult
            {
                Arguments = buildArguments,
                Failed = result.HasErrors,
                Error = result.ErrorText,
                HelpCalled = result.HelpCalled
            };
        }

        public void HelpText(Action<string> action)
        {
            _builder.SetupHelp("h", "help", "?").Callback(action);
        }
    }
}