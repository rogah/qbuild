using System.Collections.Generic;

namespace QBuild.Core
{
    public class BuildArguments
    {
        public const int DefaultPollingInterval = 5;
        public const int DefaultTimeout = 30;

        public BuildArguments()
        {
            AdditionalArguments = new Dictionary<string, string>();
        }

        public string Collection { get; set; }
        public string ProjectName { get; set; }
        public string BuildDefinition { get; set; }
        public string BuildVersion { get; set; }
        public int PollingInterval { get; set; }
        public int Timeout { get; set; }
        public bool Verbose { get; set; }
        public IEnumerable<KeyValuePair<string, string>> AdditionalArguments { get; set; }
        
    }
}
