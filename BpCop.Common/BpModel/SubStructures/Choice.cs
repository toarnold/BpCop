using System;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class Choice
    {
        public Expression Expression { get; init; }
        public string Name { get; init; }
        public Guid OnTrue { get; init; }

        internal Choice(XElement element)
        {
            Expression = new Expression(element.GetStringAttributeValue("expression"));
            Name = element.GetStringElementValue("name");
            OnTrue = element.GetTypedElementValue<Guid>("ontrue");
        }
    }
}
