using CommandLine;
using BpCop.Console.Common.Parameters;
using BpCop.Console.Common.Profiles;
using BpCop.Console.Common;

namespace BpCop.Core
{
    internal sealed class Program
    {
        static void Main(string[] args)
        {
            var functions = new Functions(System.Console.Out, System.Console.Error);
            var combinedArgs = ProfileManager.BuildArguments(args);
            Parser.Default.ParseArguments<DatabaseConnectionOptions, DatabaseNamedOptions, BpFileOptions, AutomateOptions, RuleOptions>(combinedArgs)
                .WithParsed<DatabaseConnectionOptions>(functions.CheckConnectionDatabase)
                .WithParsed<DatabaseNamedOptions>(functions.CheckNamedDatabase)
                .WithParsed<BpFileOptions>(functions.CheckFile)
                .WithParsed<AutomateOptions>(functions.CheckAutomateC)
                .WithParsed<RuleOptions>(functions.ShowRules);
        }
    }
}
