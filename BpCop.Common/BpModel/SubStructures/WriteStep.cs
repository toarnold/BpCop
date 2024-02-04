using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public class WriteStep
    {
        public Guid ElementId { get; init; }
        public Expression Expression { get; init; }
        public IList<ElementParameter> Parameter { get; init; }
        public NavigateAction? Action { get; init; }

        internal WriteStep(XElement step)
        {
            ElementId = step.Element("element").GetTypedAttributeValue<Guid>("id");
            Expression = new Expression(step.GetStringAttributeValue("expr"));
            Parameter = (step.Element("element").Elements("elementparameter")?.Select(s => new ElementParameter(s)) ?? Enumerable.Empty<ElementParameter>()).ToList();
            Action = step.Element("action") is not null ? new NavigateAction(step.Element("action")) : null;
        }
    }
}
