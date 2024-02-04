using Newtonsoft.Json;
using System.Collections.Generic;
using System.Dynamic;

namespace BpCop.Console.Common.Output
{
    internal sealed class OutputJson: IOutput
    {
        private readonly ExpandoObject expandoObject = new();

        public void Append<T>(string caption, IList<T> values)
        {
            var expandoDict = expandoObject as IDictionary<string, object>;
            expandoDict.Add(caption, values);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(expandoObject);
        }

    }
}
