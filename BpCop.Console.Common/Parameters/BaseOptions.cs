using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Parameters
{
    public enum Justification
    {
        Handle = 0, // Apply justification notes
        Off = 1, // No justification output, only findings
        Only = 2, // Output only justifications, no findings
        Ignore = 3 // Ignore justification notes, show all findings
    }

    public abstract class BaseOptions : MinimalOptions
    {
        [Option('w', "white-list", HelpText = "List of rule names to apply.")]
        public IEnumerable<string> Whitelist { get; set; } = Enumerable.Empty<string>();

        [Option('b', "black-list", HelpText = "List of rule names to skip.")]
        public IEnumerable<string> Blacklist { get; set; } = Enumerable.Empty<string>();

        [Option('j', "justifications", Default = Justification.Handle, HelpText = "Justification handling format (Handle,Off,Only,Ignore).")]
        public Justification JustificationHandling { get; set; }
    }
}
