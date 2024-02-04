using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BpCop.Console.Common.Output
{
    internal class OutputMarkdown : OutputBase, IOutput
    {
        protected string _delimiter = "|";

        public void Append<T>(string caption, IList<T> values)
        {
            var builder = new StringBuilder();

            // calc Col Size
            var columnLengths = GetColumns<T>().ToDictionary(k => k, c => Math.Max(c.Length, values.Select(v => GetColumnValue(v, c)?.ToString().Length ?? 0).DefaultIfEmpty(0).Max()));

            // set right alinment if is a number
            var columnAlignment = typeof(T).GetProperties().ToDictionary(k => k.Name, v => IsNumeric(v.PropertyType) ? string.Empty : "-");

            // create the string format with padding
            var formats = GetColumns<T>()
                .Select((name, i) => $" {_delimiter} {{{i},{columnAlignment[name] + columnLengths[name]}}}")
                .Aggregate((s, a) => s + a) + $" {_delimiter}";

            var columnHeaders = string.Format(CultureInfo.InvariantCulture, formats, GetColumns<T>().ToArray());
            var divider = Regex.Replace(columnHeaders, @"[^|]", "-", RegexOptions.Compiled);
            builder.AppendLine(columnHeaders);
            builder.AppendLine(divider);

            // add each row
            var results = values.Select(row => string.Format(CultureInfo.InvariantCulture, formats, Values(row)));
            foreach (var row in results)
            {
                builder.AppendLine(row);
            }

            _output.Add(builder.ToString());
        }
    }
}