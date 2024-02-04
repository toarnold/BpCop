using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class ChoiceStartStage : Stage, IDataFieldsRead, IExpressions
    {
        public Guid GroupId { get; init; }
        public IList<Choice> Choices { get; init; }
        public IEnumerable<string> FieldsRead => Choices.SelectMany(c => c.Expression.UsedData).Distinct();
        public override Guid OnSuccess => Page.Stages.Values.OfType<ChoiceEndStage>().First(f => f.GroupId == GroupId).StageId;
        public override IEnumerable<Guid> NextStages => base.NextStages
            .Concat(Page.Stages.Values.OfType<ChoiceEndStage>().First(f => f.GroupId == GroupId).StageId.SingleEnumerable())
            .Concat(Choices.Select(s => s.OnTrue));
        public IEnumerable<Expression> Expressions => Choices.Select(c => c.Expression);

        internal ChoiceStartStage(BpProcess process, XElement element) : base(process, element)
        {
            GroupId = element.GetTypedElementValue<Guid>("groupid");
            Choices = element.Element("choices")
                .Elements("choice")
                .Select(s => new Choice(s))
                .ToList();
        }

        /*
<stage stageid="5a173f93-ceb4-49dd-bebb-88c90ae3b412" name="Choice1" type="ChoiceStart">
  <subsheetid>aa6b3806-af47-433c-99a0-d20cf36abe26</subsheetid>
  <display x="15" y="120" />
  <groupid>13edf4e3-a43d-46f7-af9c-ffb3a6e6c051</groupid>
  <choices>
    <choice expression="[MyData]=&quot;&quot;">
      <name>1. Case</name>
      <distance>75</distance>
      <ontrue>7ce15790-8e54-4b64-88e0-3abcccb91603</ontrue>
    </choice>
  </choices>
</stage>
        */
    }
}
