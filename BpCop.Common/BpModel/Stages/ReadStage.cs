using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class ReadStage : Stage, IDataFieldsRead, IDataFieldsWrite, IAppModelFields
    {
        public IList<ReadStep> Steps { get; init; }
        public IEnumerable<string> FieldsRead => Steps.SelectMany(m => m.Parameter.SelectMany(p => p.Expression.UsedData).Union(m.Action.Input.SelectMany(i => i.Value.UsedData))).Distinct();
        public IEnumerable<string> FieldsWrite => Steps.SelectMany(m => m.Action.Output.Select(x => x.Value).Union(m.Stage.SingleEnumerable()).Where(w => !string.IsNullOrEmpty(w))).Distinct();
        public IEnumerable<Guid> AppModelFields => Steps.Select(m => m.ElementId).Distinct();

        internal ReadStage(BpProcess process, XElement element) : base(process, element)
        {
            Steps = (element.Elements("step")?.Select(s => new ReadStep(s)) ?? Enumerable.Empty<ReadStep>()).ToList();
        }

    }
}
