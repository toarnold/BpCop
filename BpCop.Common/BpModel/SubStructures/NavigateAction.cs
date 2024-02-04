using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class NavigateAction
    {
        public string Id { get; init; }
        public IList<NavigateInputArgument> Input { get; init; }
        public IList<NavigateOutputArgument> Output { get; init; }

        internal NavigateAction(XElement action)
        {
            Id = action.GetStringElementValue("id");
            Input = (action.Element("arguments")?.Elements("argument").Select(s => new NavigateInputArgument(s)) ?? Enumerable.Empty<NavigateInputArgument>()).ToList();
            Output = (action.Element("outputs")?.Elements("output").Select(s => new NavigateOutputArgument(s)) ?? Enumerable.Empty<NavigateOutputArgument>()).ToList();
        }
    }
}
