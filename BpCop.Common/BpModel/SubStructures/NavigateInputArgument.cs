using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class NavigateInputArgument
    {
        public string Id { get; init; }
        public Expression Value { get; init; }

        internal NavigateInputArgument(XElement argument)
        {
            Value = new Expression(argument.GetStringElementValue("value"));
            Id = argument.GetStringElementValue("id");
        }
    }
}
