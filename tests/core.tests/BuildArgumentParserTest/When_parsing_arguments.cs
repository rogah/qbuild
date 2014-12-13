using System;
using System.Text;
using Xunit;

namespace QBuild.Core.Tests.BuildArgumentParserTest
{
    public class When_parsing_arguments
    {
        private readonly String _helpText = String.Join(String.Empty, new[]
        {
            Environment.NewLine,
            "\t", "c:collection", "\t\t", "Collection url (e.g. https://tfs.domain.com:8080/tfs/foo).", "\n",
            "\t", "d:definition", "\t\t", "Build definition name (e.g. ProjectName-Relese#).", "\n",
            "\t", "i:interval", "\t\t", "Polling interval in seconds. Default is 5.", "\n",
            "\t", "p:project", "\t\t", "Project name (e.g. Project-Name).", "\n",
            "\t", "t:timeout", "\t\t", "Timeout in minutes. Default is 30.", "\n",
            "\t", "v:version", "\t\t", "Build version (e.g. ProjectName-20140110.1).", "\n",
            "\t", "x:verbose", "\t\t", "Verbose logs. Default is false.", "\n"
        });

        private readonly IArgumentParser _parser;

        public When_parsing_arguments()
        {
            _parser = new BuildArgumentParser();
        }

        [Fact]
        public void Should_return_failed_result_if_no_argumet_is_provided()
        {
            var arguments = new string[0];

            var result = _parser.Parse(arguments);

            Assert.True(result.Failed);
        }

        [Fact]
        public void Should_return_error_message_if_no_argumet_is_provided()
        {
            var arguments = new string[0];

            var expected = new StringBuilder();
            expected.AppendLine("Option 'c:collection' parse error. option is required but was not specified.");
            expected.AppendLine("Option 'p:project' parse error. option is required but was not specified.");
            expected.AppendLine("Option 'd:definition' parse error. option is required but was not specified.");
            expected.AppendLine("Option 'v:version' parse error. option is required but was not specified.");

            var result = _parser.Parse(arguments);

            Assert.Equal(expected.ToString(), result.Error);
        }

        [Fact]
        public void Should_return_failed_result_if_collection_is_not_provided()
        {
            var arguments = new[] { "--project", "foo", "--definition", "bar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.True(result.Failed);
        }

        [Fact]
        public void Should_return_error_message_if_collection_is_not_provided()
        {
            var arguments = new[] { "--project", "foo", "--definition", "bar", "--version", "12345" };

            var expected = new StringBuilder().AppendLine("Option 'c:collection' parse error. option is required but was not specified.").ToString();

            var result = _parser.Parse(arguments);

            Assert.Equal(expected, result.Error);
        }

        [Fact]
        public void Should_parse_collection_argument()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.Collection);
        }

        [Fact]
        public void Should_parse_collection_alias()
        {
            var arguments = new[] { "-c", "http://tfs.dell.com:8080/tfs/collection" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.Collection);
        }

        [Fact]
        public void Should_return_failed_result_if_project_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--definition", "foo", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.True(result.Failed);
        }

        [Fact]
        public void Should_return_error_message_if_project_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--definition", "foo", "--version", "12345" };

            var expected = new StringBuilder().AppendLine("Option 'p:project' parse error. option is required but was not specified.").ToString();

            var result = _parser.Parse(arguments);

            Assert.Equal(expected, result.Error);
        }

        [Fact]
        public void Should_parse_project_argument()
        {
            var arguments = new[] { "--project", "foo" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.ProjectName);
        }

        [Fact]
        public void Should_parse_project_alias()
        {
            var arguments = new[] { "-p", "foo" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.ProjectName);
        }

        [Fact]
        public void Should_return_failed_result_if_build_definition_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.True(result.Failed);
        }

        [Fact]
        public void Should_return_error_message_if_build_definition_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--version", "12345" };

            var expected = new StringBuilder().AppendLine("Option 'd:definition' parse error. option is required but was not specified.").ToString();

            var result = _parser.Parse(arguments);

            Assert.Equal(expected, result.Error);
        }

        [Fact]
        public void Should_parse_build_definition_argument()
        {
            var arguments = new[] { "--definition", "foo" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.BuildDefinition);
        }

        [Fact]
        public void Should_parse_build_definition_alias()
        {
            var arguments = new[] { "-d", "foo" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.BuildDefinition);
        }

        [Fact]
        public void Should_return_failed_result_if_build_version_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "bar" };

            var result = _parser.Parse(arguments);

            Assert.True(result.Failed);
        }

        [Fact]
        public void Should_return_error_message_if_build_version_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "foobar" };

            var expected = new StringBuilder().AppendLine("Option 'v:version' parse error. option is required but was not specified.").ToString();

            var result = _parser.Parse(arguments);

            Assert.Equal(expected, result.Error);
        }

        [Fact]
        public void Should_parse_build_version_argument()
        {
            var arguments = new[] { "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.BuildVersion);
        }

        [Fact]
        public void Should_parse_build_version_alias()
        {
            var arguments = new[] { "-v", "12345" };

            var result = _parser.Parse(arguments);

            Assert.Equal(arguments[1], result.Arguments.BuildVersion);
        }

        [Fact]
        public void Should_not_return_failed_result_if_polling_interval_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "bar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.False(result.Failed);
        }

        [Fact]
        public void Should_not_return_error_message_if_polling_interval_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "foobar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.Null(result.Error);
        }

        [Fact]
        public void Should_parse_polling_interval_argument_with_default_value_if_not_provided()
        {
            var arguments = new string[0];

            var result = _parser.Parse(arguments);

            Assert.Equal(BuildArguments.DefaultPollingInterval, result.Arguments.PollingInterval);
        }

        [Fact]
        public void Should_parse_polling_interval_argument()
        {
            var arguments = new[] { "--interval", "10" };

            var result = _parser.Parse(arguments);

            Assert.Equal(10, result.Arguments.PollingInterval);
        }

        [Fact]
        public void Should_parse_polling_interval_alias()
        {
            var arguments = new[] { "-i", "15" };

            var result = _parser.Parse(arguments);

            Assert.Equal(15, result.Arguments.PollingInterval);
        }

        [Fact]
        public void Should_not_return_failed_result_if_timeout_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "bar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.False(result.Failed);
        }

        [Fact]
        public void Should_not_return_error_message_if_timeout_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "foobar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.Null(result.Error);
        }

        [Fact]
        public void Should_parse_timeout_argument_with_default_value_if_not_provided()
        {
            var arguments = new string[0];

            var result = _parser.Parse(arguments);

            Assert.Equal(BuildArguments.DefaultTimeout, result.Arguments.Timeout);
        }

        [Fact]
        public void Should_parse_timeout_argument()
        {
            var arguments = new[] { "--timeout", "10" };

            var result = _parser.Parse(arguments);

            Assert.Equal(10, result.Arguments.Timeout);
        }

        [Fact]
        public void Should_parse_timeout_alias()
        {
            var arguments = new[] { "-t", "15" };

            var result = _parser.Parse(arguments);

            Assert.Equal(15, result.Arguments.Timeout);
        }

        [Fact]
        public void Should_not_return_failed_result_if_verbose_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "bar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.False(result.Failed);
        }

        [Fact]
        public void Should_not_return_error_message_if_tverbose_is_not_provided()
        {
            var arguments = new[] { "--collection", "http://tfs.dell.com:8080/tfs/collection", "--project", "foo", "--definition", "foobar", "--version", "12345" };

            var result = _parser.Parse(arguments);

            Assert.Null(result.Error);
        }

        [Fact]
        public void Should_parse_verbose_argument_with_default_value_if_not_provided()
        {
            var arguments = new string[0];

            var result = _parser.Parse(arguments);

            Assert.False(result.Arguments.Verbose);
        }

        [Fact]
        public void Should_parse_verbose_argument()
        {
            var arguments = new[] { "--verbose" };

            var result = _parser.Parse(arguments);

            Assert.True(result.Arguments.Verbose);
        }

        [Fact]
        public void Should_parse_verbose_alias()
        {
            var arguments = new[] { "-x" };

            var result = _parser.Parse(arguments);

            Assert.True(result.Arguments.Verbose);
        }

        [Fact]
        public void Should_parse_any_aditional_argument_name()
        {
            var arguments = new[] { "--foo", "bar", "--baz", "qux", "--bool" };

            var expctedArgumentNames = new[] { "foo", "baz", "bool" };

            var result = _parser.Parse(arguments);

            foreach (var additionalArgument in result.Arguments.AdditionalArguments)
                Assert.Contains(additionalArgument.Key, expctedArgumentNames);
        }

        [Fact]
        public void Should_parse_any_aditional_argument_value()
        {
            var arguments = new[] { "--foo", "bar", "--baz", "qux", "--bool" };

            var expctedArgumentValue = new object[] { "bar", "qux", null };

            var result = _parser.Parse(arguments);

            foreach (var additionalArgument in result.Arguments.AdditionalArguments)
                Assert.Contains(additionalArgument.Value, expctedArgumentValue);
        }

        [Fact]
        public void Should_display_help_text_if_help_argument_provided()
        {
            var arguments = new[] { "--help" };

            _parser.HelpText(message => Assert.Equal(_helpText, message));

            _parser.Parse(arguments);
        }

        [Fact]
        public void Should_return_true_for_help_called_if_help_argument_provided()
        {
            var arguments = new[] { "--help" };

            _parser.HelpText(message => { });

            var result = _parser.Parse(arguments);

            Assert.True(result.HelpCalled);
        }

        [Fact]
        public void Should_display_help_text_if_help_alias_provided()
        {
            var arguments = new[] { "-h" };

            _parser.HelpText(message => Assert.Equal(_helpText, message));

            _parser.Parse(arguments);
        }

        [Fact]
        public void Should_return_true_for_help_called_if_help_alias_provided()
        {
            var arguments = new[] { "-h" };

            _parser.HelpText(message => { });

            var result = _parser.Parse(arguments);

            Assert.True(result.HelpCalled);
        }

        [Fact]
        public void Should_display_help_text_if_question_mark_argument_provided()
        {
            var arguments = new[] { "/?" };

            _parser.HelpText(message => Assert.Equal(_helpText, message));

            _parser.Parse(arguments);
        }

        [Fact]
        public void Should_return_true_for_help_called_if_question_mark_argument_provided()
        {
            var arguments = new[] { "/?" };

            _parser.HelpText(message => { });

            var result = _parser.Parse(arguments);

            Assert.True(result.HelpCalled);
        }
    }
}