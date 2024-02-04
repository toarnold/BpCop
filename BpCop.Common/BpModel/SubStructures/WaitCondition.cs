using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class WaitCondition
    {
        public string Id { get; init; }
        public IList<WaitConditionArgument> Arguments { get; init; }

        internal WaitCondition(XElement condition)
        {
            Id = condition.GetStringElementValue("id");
            Arguments = condition.Elements("arguments").Select(s => new WaitConditionArgument(s.Element("argument"))).ToList();
        }
    }
}
