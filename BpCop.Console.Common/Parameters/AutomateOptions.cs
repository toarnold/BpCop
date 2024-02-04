using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Parameters
{
    [Verb("check-automate", HelpText = "Load assets from Blue Prism AutomateC.")]
    public sealed class AutomateOptions : BaseOptions
    {
        [Option('a', "assets", Required = true, HelpText = "Process or object names to analyse.")]
        public IEnumerable<string> ProcessNames { get; set; } = Enumerable.Empty<string>();

        [Option("automatec", HelpText = "Path to automate.exe", Default = @"C:\Program Files\Blue Prism Limited\Blue Prism Automate\AutomateC.exe")]
        public string AutomateC { get; set; } = string.Empty;

        [Option('c', "connection", Default = "", HelpText = "Connection name to use.")]
        public string ConnectionName { get; set; } = string.Empty;

        [Option('u', "user", HelpText = "Blue Prism user name (leave blank for sso).", Default = "")]
        public string User { get; set; } = string.Empty;

        [Option('p', "password", HelpText = "Blue Prism user password.", Default = "")]
        public string Password { get; set; } = string.Empty;
    }
}
