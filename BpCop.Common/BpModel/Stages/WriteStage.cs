using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class WriteStage : Stage, IDataFieldsRead, IAppModelFields, IExpressions
    {
        public IList<WriteStep> Steps { get; init; }
        public IEnumerable<string> FieldsRead => Steps.SelectMany(m => m.Expression.UsedData.Union(m.Parameter.SelectMany(p => p.Expression.UsedData)).Union(m.Action?.Input.SelectMany(i => i.Value.UsedData) ?? Enumerable.Empty<string>())).Distinct();
        public IEnumerable<Guid> AppModelFields => Steps.Select(m => m.ElementId).Distinct();
        public IEnumerable<Expression> Expressions => Steps.Select(s => s.Expression);

        internal WriteStage(BpProcess process, XElement element) : base(process, element)
        {
            Steps = (element.Elements("step")?.Select(s => new WriteStep(s)) ?? Enumerable.Empty<WriteStep>()).ToList();
        }
    }
}
