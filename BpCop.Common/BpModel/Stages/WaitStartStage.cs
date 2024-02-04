using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class WaitStartStage : Stage, IDataFieldsRead, IAppModelFields, IExpressions
    {
        public Guid GroupId { get; init; }
        public Expression Timeout { get; init; }
        public IList<WaitChoice> Choices { get; init; }
        public override Guid OnSuccess => Page.Stages.Values.OfType<WaitEndStage>().First(f => f.GroupId == GroupId).StageId;
        public override IEnumerable<Guid> NextStages => base.NextStages
            .Concat(Page.Stages.Values.OfType<WaitEndStage>().First(f => f.GroupId == GroupId).StageId.SingleEnumerable())
            .Concat(Choices.Select(s => s.OnTrue));

        public IEnumerable<string> FieldsRead => Timeout.UsedData
            .Union(Choices.SelectMany(c => c.FieldsRead))
            .Union(Choices.SelectMany(m => m.WaitCondition.Arguments.SelectMany(a => a.Value.UsedData)));

        public IEnumerable<Guid> AppModelFields => Choices.Select(c => c.ElementId).Distinct();

        public IEnumerable<Expression> Expressions => Choices.Select(c => c.Value);

        internal WaitStartStage(BpProcess process, XElement element) : base(process, element)
        {
            GroupId = element.GetTypedElementValue<Guid>("groupid");
            Timeout = new Expression(element.GetStringElementValue("timeout"));
            Choices = element.Element("choices").Elements("choice").Select(c => new WaitChoice(c)).ToList();
        }
    }
}
