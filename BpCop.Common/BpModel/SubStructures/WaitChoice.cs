using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.SubStructures
{
    public sealed class WaitChoice
    {
        public IList<ElementParameter> Parameter { get; init; }
        public Expression Value { get; init; }
        public decimal Distance { get; init; }
        public string Name { get; init; }
        public Guid OnTrue { get; init; }
        public WaitCondition WaitCondition { get; init; }
        public Guid ElementId { get; init; }
        public IEnumerable<string> FieldsRead => Value.UsedData.Union(Parameter.SelectMany(p => p.Expression.UsedData));

        internal WaitChoice(XElement element)
        {
            Distance = element.GetTypedElementValue<decimal>("distance");
            Name = element.GetStringElementValue("name");
            OnTrue = element.GetTypedElementValue<Guid>("ontrue");
            Value = new Expression(element.GetStringAttributeValue("reply"));
            Parameter = (element.Element("element")?.Elements("elementparameter")?.Select(s => new ElementParameter(s)) ?? Enumerable.Empty<ElementParameter>()).ToList();
            WaitCondition = new WaitCondition(element.Element("condition"));
            ElementId = element.Element("element")?.GetTypedAttributeValue<Guid>("id") ?? Guid.Empty;
        }


        /*
            <choice reply="True">
				<name>Content Parent Document Loaded</name>
				<distance>60</distance>
				<ontrue>7dc8a13b-84bc-4d19-b6bc-b214a53b2f73</ontrue>
				<element id="d09918a0-5bf0-48cd-90d8-f281dd2744d8" />
				<condition>
					<id>WebCheckParentDocumentLoaded</id>
					<arguments>
						<argument>
							<id>trackingid</id>
							<value></value>
						</argument>
					</arguments>
				</condition>
				<comparetype>Equal</comparetype>
			</choice>
         */
    }
}
