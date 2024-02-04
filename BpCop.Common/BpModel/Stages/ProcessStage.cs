using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class ProcessStage : Stage, IDataFieldsRead, IDataFieldsWrite, IExpressions, IDataSinks, IDataSources
    {
        public Guid ProcessId { get; init; }
        public IEnumerable<DataSource> DataSources { get; init; }
        public IEnumerable<DataSink> DataSinks { get; init; }
        public IEnumerable<string> FieldsRead => DataSources.SelectMany(s => s.Expression.UsedData).Distinct();
        public IEnumerable<string> FieldsWrite => DataSinks.Select(s => s.Stage).Where(w => !string.IsNullOrEmpty(w)).Distinct();
        public IEnumerable<Expression> Expressions => DataSources.Select(s => s.Expression);

        internal ProcessStage(BpProcess process, XElement element) : base(process, element)
        {
            ProcessId = element.GetTypedElementValue<Guid>("processid");
            DataSources = DataSource.ToEnumerable(element.Element("inputs")?.Elements("input"));
            DataSinks = DataSink.ToEnumerable(element.Element("outputs")?.Elements("output"));
        }
    }
}
