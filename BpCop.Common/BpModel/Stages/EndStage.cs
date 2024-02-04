using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class EndStage : Stage, IDataFieldsRead, IDataSinks
    {
        public IEnumerable<DataSink> DataSinks { get; init; }
        public IEnumerable<string> FieldsRead => DataSinks.Select(s => s.Stage).Where(w => !string.IsNullOrEmpty(w)).Distinct();

        internal EndStage(BpProcess process, XElement element) : base(process, element)
        {
            DataSinks = DataSink.ToEnumerable(element.Element("outputs")?.Elements("output"));
        }
    }
}
