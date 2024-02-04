using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class Calculation
    {
        public Expression Expression { get; init; }
        public string Stage { get; init; }

        internal Calculation(XElement element)
        {
            Expression = new Expression(element.GetStringAttributeValue("expression"));
            Stage = element.GetStringAttributeValue("stage");
        }
    }
}
