using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Output
{
    internal sealed class OutputCsv : OutputBase, IOutput
    {
        public void Append<T>(string caption, IList<T> values)
        {
            // Headline
            _output.Add(string.Join(";", GetColumns<T>()));

            // Values
            foreach (var propertyValues in values.Select(value => GetColumns<T>().Select(column => GetColumnValue(value, column))))
            {
                _output.Add(string.Join(";", propertyValues));
            }
        }
    }
}
