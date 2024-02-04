using System;
using System.Collections.Generic;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class DecisionStage : Stage, IDataFieldsRead, IExpressions
    {
        public Expression Expression { get; init; }
        public override Guid OnSuccess { get; init; }
        public Guid OnFalse { get; init; }
        public IEnumerable<string> FieldsRead => Expression.UsedData;
        public override IEnumerable<Guid> NextStages => new[] { OnSuccess, OnFalse };
        public IEnumerable<Expression> Expressions => Expression.SingleEnumerable();

        internal DecisionStage(BpProcess process, XElement element) : base(process, element)
        {
            Expression = new Expression(element.Element("decision").GetStringAttributeValue("expression"));
            OnSuccess = element.GetTypedElementValue<Guid>("ontrue");
            OnFalse = element.GetTypedElementValue<Guid>("onfalse");
        }
    }
}
