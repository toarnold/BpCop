using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class StartStage : Stage, IDataFieldsWrite, IDataSinks
    {
        public IEnumerable<DataSink> DataSinks { get; init; }
        public IEnumerable<string> FieldsWrite => DataSinks.Select(s => s.Stage).Where(w => !string.IsNullOrEmpty(w)).Distinct();
        public IEnumerable<string> PreConditions => StageElement.Element("preconditions")?.Elements("condition").Select(s => s.Attribute("narrative").Value) ?? Enumerable.Empty<string>();
        public IEnumerable<string> PostConditions => StageElement.Element("postconditions")?.Elements("condition").Select(s => s.Attribute("narrative").Value) ?? Enumerable.Empty<string>();

        internal StartStage(BpProcess process, XElement element) : base(process, element)
        {
            DataSinks = DataSink.ToEnumerable(element.Element("inputs")?.Elements("input"));
        }
    }
}
