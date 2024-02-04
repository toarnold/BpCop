using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public class WaitConditionArgument
    {
        public string Id { get; init; }
        public Expression Value { get; init; }

        internal WaitConditionArgument(XElement argument)
        {
            Id = argument.GetStringElementValue("id");
            Value = new Expression(argument.GetStringElementValue("value"));
        }
    }
}
