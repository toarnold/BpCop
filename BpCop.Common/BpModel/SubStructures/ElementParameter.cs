using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class ElementParameter
    {
        public string Name { get; init; }
        public Expression Expression { get; init; }
        public string DataType { get; init; }
        
        internal ElementParameter(XElement parameter)
        {
            Name = parameter.GetStringElementValue("name");
            DataType = parameter.GetStringElementValue("datatype");
            Expression = new Expression(parameter.GetStringElementValue("expression"));
        }
    }
}
