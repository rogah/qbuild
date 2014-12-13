using System;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using QBuild.Core;

namespace QBuild
{
    static class Program
    {
        static int Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.Out.WriteLine("{0} (version {1})\n", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            
            IArgumentParser parser = new BuildArgumentParser();

            try
            {
                parser.HelpText(PrintHelp);

                var result = parser.Parse(args);

                if (result.HelpCalled) return (int) Status.Success;

                if (result.Failed) return HandleParseIssues(result);

                var collection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(
                    new Uri(result.Arguments.Collection));

                IBuildService buildService = new BuildService(collection);

                var request = buildService.CreateRequest(result.Arguments);

                var status = buildService.Build(request, HandleReponse);

                return HandleBuildStatus(status);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        private static int HandleBuildStatus(BuildStatus status)
        {
            if (BuildStatus.Failed == status) return (int) Status.Failed;
            if (BuildStatus.PartiallySucceeded == status) return (int) Status.PartiallySucceeded;
            if (BuildStatus.NotStarted == status) return (int) Status.NotStarted;
            if (BuildStatus.Stopped == status) return (int) Status.Stopped;
            if (BuildStatus.None == status) return (int) Status.None;

            return (int) Status.Success;
        }

        private static void HandleReponse(IBuildResponse response)
        {
            Console.Out.WriteLine(response.Status.ToString());

            if (!String.IsNullOrEmpty(response.Output))
                Console.Out.WriteLine(response.Output);
        }

        private static void PrintHelp(string text)
        {
            Console.Out.WriteLine("Usage:\nqbuild -c <collection> -p<project> -d <definition> -v <version> [-i <interval>] [-t <timeout>] [-x] [--<parameter_name> <parameter_value> [--<parameter_name> <parameter_value> [...]]]");
            Console.Out.WriteLine(text);
        }

        private static int HandleParseIssues(IParseResult result)
        {
            if (!String.IsNullOrEmpty(result.Error))
                Console.Error.WriteLine(result.Error);
            return (int) Status.ParseFailed;
        }

        private static int HandleException(Exception exception)
        {
            Console.Error.WriteLine(exception.Message);
            return (int) Status.Failed;
        }
    }
}
