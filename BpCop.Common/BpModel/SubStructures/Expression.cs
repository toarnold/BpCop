using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class Expression
    {
        private static IEnumerable<string> GetParts(string val)
        {
            int p = 0;
            int start = 0;
            bool inQuotes = false;
            while (p < val.Length)
            {
                switch (val[p++])
                {
                    case '"':
                        inQuotes = !inQuotes;
                        break;
                    case '&':
                        if (!inQuotes)
                        {
                            yield return p - start - 1 > 0 ? val.Substring(start, p - start - 1) : string.Empty;
                            start = p;
                        }
                        break;
                }
            }
            yield return start < val.Length ? val.Substring(start) : string.Empty;
        }

        private static readonly Regex DataExpression = new(@"\[(?<Name>.+?)\]", RegexOptions.Compiled);
        public string Value { get; init; }
        public IList<string> UsedData { get; init; }
        public IList<string> Parts { get; init; }

        internal Expression(string expression)
        {
            Value = expression;
            Parts = GetParts(Value).ToList();
            UsedData = Parts
                .Where(w => !w.TrimStart().StartsWith("\"", StringComparison.InvariantCulture)) // Don't analyse strings
                .SelectMany(p => DataExpression.Matches(p).Cast<Match>().Select(m => m.Groups["Name"].Value))
                .Distinct()
                .ToList();
        }
    }
}
