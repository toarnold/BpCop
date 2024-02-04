using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class DataSource
    {
        public string Name { get; init; }
        public string DataType { get; init; }
        public string Narrative { get; init; }
        public Expression Expression { get; init; }

        private DataSource(XElement element)
        {
            Expression = new Expression(element.GetStringAttributeValue("expr"));
            Name = element.GetStringAttributeValue("name");
            DataType = element.GetStringAttributeValue("type");
            Narrative = element.GetStringAttributeValue("narrative");
        }

        internal static IEnumerable<DataSource> ToEnumerable(IEnumerable<XElement>? elements)
        {
            return elements?.Select(s => new DataSource(s)) ?? Enumerable.Empty<DataSource>();
        }
    }
}
