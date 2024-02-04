using BpCop.Console.Common.Output;
using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Parameters
{
    public class MinimalOptions
    {
        [Option("profile", Default = "", HelpText = "app.config stored profile name")]
        public string Profile { get; set; } = string.Empty;

        [Option('r', "rule-folders", Default = new[] { "Rules" }, HelpText = "Relative paths to the ruleset folder(s).")]
        public IEnumerable<string> RuleFolders { get; set; } = Enumerable.Empty<string>();

        [Option('c', "custom-parameter", Group = "Parameter", Default = "{}", HelpText = "JSON object with custom parameters for customisable rules.")]
        public string CustomParameter { get; set; } = string.Empty;

        [Option("named-custom-parameter", Group = "Parameter", HelpText = "Named JSON object with custom parameters for customisable rules from app.config.")]
        public string NamedCustomParameter { get; set; } = string.Empty;

        [Option('o', "output-format", Default = Format.Default, HelpText = "Output format (Default,Markdown,Minimal,Csv,Json,Html).")]
        public Format OutputFormat { get; set; }
    }
}
