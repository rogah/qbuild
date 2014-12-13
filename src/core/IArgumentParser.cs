using System;

namespace QBuild.Core
{
    public interface IArgumentParser
    {
        IParseResult Parse(string[] arguments);
        void HelpText(Action<string> action);
    }
}