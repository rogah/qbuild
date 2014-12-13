namespace QBuild.Core
{
    public class ParseResult : IParseResult
    {
        public BuildArguments Arguments { get; set; }
        public bool Failed { get; set; }
        public string Error { get; set; }
        public bool HelpCalled { get; set; }
    }
}