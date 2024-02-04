using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class Display
    {
        public int X { get; init; }
        public int Y { get; init; }
        public int W { get; init; }
        public int H { get; init; }

        internal Display(XElement element, bool deprecated = false)
        {
            X = deprecated ? element.GetTypedElementValue<int>("displayx") : element.GetTypedAttributeValue<int>("x");
            Y = deprecated ? element.GetTypedElementValue<int>("displayy") : element.GetTypedAttributeValue<int>("y");
            W = deprecated ? element.GetTypedElementValue<int>("displaywidth") : element.GetTypedAttributeValue<int>("w");
            H = deprecated ? element.GetTypedElementValue<int>("displayheight") : element.GetTypedAttributeValue<int>("h");
        }
    }

}
