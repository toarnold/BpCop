using BpCop.Common.BpModel;
using BpCop.Console.Common.Parameters;
using CommandLine;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BpCop.Console.Common.Profiles
{
    public static class ProfileManager
    {
        // Taken from here: https://github.com/dotnet/roslyn/blob/version-3.2.0/src/Compilers/Core/Portable/InternalUtilities/CommandLineUtilities.cs
        private static List<string> SplitCommandLineIntoArguments(string commandLine)
        {
            var builder = new StringBuilder(commandLine.Length);
            var list = new List<string>();
            var i = 0;

            while (i < commandLine.Length)
            {
                while (i < commandLine.Length && char.IsWhiteSpace(commandLine[i]))
                {
                    i++;
                }

                if (i == commandLine.Length)
                {
                    break;
                }

                var quoteCount = 0;
                builder.Length = 0;
                while (i < commandLine.Length && (!char.IsWhiteSpace(commandLine[i]) || (quoteCount % 2 != 0)))
                {
                    var current = commandLine[i];
                    switch (current)
                    {
                        case '\\':
                            {
                                var slashCount = 0;
                                do
                                {
                                    builder.Append(commandLine[i]);
                                    i++;
                                    slashCount++;
                                } while (i < commandLine.Length && commandLine[i] == '\\');

                                // Slashes not followed by a quote character can be ignored for now
                                if (i >= commandLine.Length || commandLine[i] != '"')
                                {
                                    break;
                                }

                                // If there is an odd number of slashes then it is escaping the quote
                                // otherwise it is just a quote.
                                if (slashCount % 2 == 0)
                                {
                                    quoteCount++;
                                }

                                builder.Append('"');
                                i++;
                                break;
                            }

                        case '"':
                            builder.Append(current);
                            quoteCount++;
                            i++;
                            break;

                        default:
                            if ((current < 0x1 || current > 0x1f) && current != '|')
                            {
                                builder.Append(current);
                            }

                            i++;
                            break;
                    }
                }

                // If the quote string is surrounded by quotes with no interior quotes then 
                // remove the quotes here. 
                if (quoteCount == 2 && builder[0] == '"' && builder[builder.Length - 1] == '"')
                {
                    builder.Remove(0, length: 1);
                    builder.Remove(builder.Length - 1, length: 1);
                }

                if (builder.Length > 0)
                {
                    list.Add(builder.ToString());
                }
            }

            return list;
        }

        public static IEnumerable<string> BuildArguments(string[] args)
        {
            var extArgs = Enumerable.Empty<string>();
            using var parser = new Parser(settings => { settings.IgnoreUnknownArguments = true; });
            var minimalOptions = parser.ParseArguments<MinimalOptions>(args);
            if (minimalOptions.Tag == ParserResultType.Parsed)
            {
                if (ConfigurationManager.GetSection("Profiles") is IDictionary<string, Dictionary<string, string>> profiles && profiles.TryGetValue(minimalOptions.Value.Profile, out Dictionary<string, string>? value))
                {
                    extArgs = value.Where(w => !args.Contains(w.Key)) // Remove overridden parameters
                        .SelectMany(pair => pair.Key.SingleEnumerable().Concat(SplitCommandLineIntoArguments(pair.Value)));
                }
            }
            return args.Concat(extArgs);
        }
    }
}
