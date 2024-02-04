using CommandLine;

namespace BpCop.Console.Common.Parameters
{
    [Verb("show-rules", HelpText = "Display all available rules.")]
    public sealed class RuleOptions : MinimalOptions
    {
    }
}
