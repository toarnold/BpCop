using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BpCop.Console.Common.Output
{
    internal sealed class OutputDefault : OutputBase, IOutput
    {
        public void Append<T>(string caption, IList<T> values)
        {
            var builder = new StringBuilder();

            // calc Col Size
            var columnLengths = GetColumns<T>().ToDictionary(k => k, c => Math.Max(c.Length, values.Select(v => GetColumnValue(v, c)?.ToString().Length ?? 0).DefaultIfEmpty(0).Max()));

            // set right alinment if is a number
            var columnAlignment = typeof(T).GetProperties().ToDictionary(k => k.Name, v => IsNumeric(v.PropertyType) ? string.Empty : "-");

            // create the string format with padding
            var formats = GetColumns<T>()
                .Select((name, i) => $" | {{{i},{columnAlignment[name] + columnLengths[name]}}}")
                .Aggregate((s, a) => s + a) + " |";

            // find the longest formatted line
            var maxRowLength = Math.Max(0, values.Any() ? values.Max(row => string.Format(CultureInfo.InvariantCulture, formats, Values(row)).Length) : 0);
            var columnHeaders = string.Format(CultureInfo.InvariantCulture, formats, GetColumns<T>().ToArray());

            // longest line is greater of formatted columnHeader and longest row
            var longestLine = Math.Max(maxRowLength, columnHeaders.Length);

            // add each row
            var results = values.Select(row => string.Format(CultureInfo.InvariantCulture, formats, Values(row)));

            // create the divider
            var divider = $" {new string('-', longestLine - 1)} ";

            builder.AppendLine(divider);
            builder.AppendLine(columnHeaders);

            foreach (var row in results)
            {
                builder.AppendLine(divider);
                builder.AppendLine(row);
            }

            builder.AppendLine(divider);

            builder.AppendLine();
            builder.AppendFormat(CultureInfo.InvariantCulture, " Count: {0}", values.Count);

            _output.Add(builder.ToString());
        }
    }
}