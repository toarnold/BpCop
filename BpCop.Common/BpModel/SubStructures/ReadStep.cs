using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class ReadStep
    {
        public Guid ElementId { get; init; }
        public string Stage { get; init; }
        public IList<ElementParameter> Parameter { get; init; }
        public NavigateAction Action { get; init; }

        internal ReadStep(XElement step)
        {
            ElementId = step.Element("element").GetTypedAttributeValue<Guid>("id");
            Stage = step.GetStringAttributeValue("stage");
            Parameter = (step.Element("element")?.Elements("elementparameter")?.Select(s => new ElementParameter(s)) ?? Enumerable.Empty<ElementParameter>()).ToList();
            Action = new NavigateAction(step.Element("action"));
        }
    }
}
