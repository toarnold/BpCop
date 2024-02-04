using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class NavigateOutputArgument
    {
        public string Id { get; init; }
        public string Value { get; init; }

        internal NavigateOutputArgument(XElement argument)
        {
            Id = argument.GetStringElementValue("id");
            Value = argument.GetStringElementValue("value");
        }
    }
}
