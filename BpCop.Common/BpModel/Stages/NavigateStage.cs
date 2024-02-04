using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class NavigateStage : Stage, IDataFieldsRead, IDataFieldsWrite, IAppModelFields, IExpressions
    {
        public IList<NavigateStep> Steps { get; init; }

        public IEnumerable<string> FieldsRead => Steps.SelectMany(m => m.Parameter.SelectMany(x => x.Expression.UsedData).Union(m.Action.Input.SelectMany(y => y.Value.UsedData))).Distinct();
        public IEnumerable<string> FieldsWrite => Steps.SelectMany(m => m.Action.Output.Select(x => x.Value).Where(w => !string.IsNullOrEmpty(w))).Distinct();
        public IEnumerable<Guid> AppModelFields => Steps.Select(m => m.ElementId).Distinct();
        public IEnumerable<Expression> Expressions => Steps.SelectMany(s => s.Parameter.Select(p => p.Expression));

        internal NavigateStage(BpProcess process, XElement element) : base(process, element)
        {
            Steps = (element.Elements("step")?.Select(s => new NavigateStep(s)) ?? Enumerable.Empty<NavigateStep>()).ToList();
        }
    }
}
