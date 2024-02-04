using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Parameters
{
    [Verb("check-db", HelpText = "Load assets from the Blue Prism database.")]
    public abstract class DatabaseOptions : BaseOptions
    {
        [Option('a', "assets", SetName = "assets", HelpText = "Process or object names to analyse (leave empty for all assets). ")]
        public IEnumerable<string> ProcessNames { get; set; } = Enumerable.Empty<string>();

        [Option('i', "ids", SetName = "assets", HelpText = "Asset guid(s) to analyse.")]
        public IEnumerable<Guid> AssetIds { get; set; } = Enumerable.Empty<Guid>();

        [Option("asset-filter", SetName = "assets", HelpText = "Database asset name 'LIKE' expression.")]
        public string AssetFilter { get; set; } = string.Empty;

        [Option("show-tree", SetName = "tree", HelpText = "No checks, exports asset tree only.")]
        public bool ShowTree { get; set; }
    }
}
