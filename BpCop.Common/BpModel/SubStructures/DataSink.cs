using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class DataSink
    {
        public string Name { get; init; }
        public string DataType { get; init; }
        public string Narrative { get; init; }
        public string Stage { get; init; }

        private DataSink(XElement element)
        {
            Name = element.GetStringAttributeValue("name");
            DataType = element.GetStringAttributeValue("type");
            Narrative = element.GetStringAttributeValue("narrative");
            Stage = element.GetStringAttributeValue("stage");
        }

        internal static IEnumerable<DataSink> ToEnumerable(IEnumerable<XElement>? elements)
        {
            return elements?.Select(s => new DataSink(s)) ?? Enumerable.Empty<DataSink>();
        }

    }
}
