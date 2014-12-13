namespace QBuild.Core
{
    public interface IParseResult
    {
        BuildArguments Arguments { get; }
        bool Failed { get; }
        string Error { get; }
        bool HelpCalled { get; }
    }
}