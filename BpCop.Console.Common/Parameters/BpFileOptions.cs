using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Parameters
{
    [Verb("check-file", HelpText = "Checks a Blue Prism .bpobject, .bpprocess or .bprelease file.")]
    public sealed class BpFileOptions : BaseOptions
    {
        [Option('f', "file", Required = true, HelpText = "Blue Prism object, process or release file.")]
        public string File { get; set; } = string.Empty;

        [Option('a', "assets", HelpText = "Process or object names to analyse (leave empty for all assets in a release file).")]
        public IEnumerable<string> ProcessNames { get; set; } = Enumerable.Empty<string>();
    }
}
