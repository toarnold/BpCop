using System.Collections.Generic;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class AlertStage : Stage, IDataFieldsRead, IExpressions
    {
        /// <summary>
        /// Used expression
        /// </summary>
        public Expression Expression { get; init; }

        /// <summary>
        /// Enumerates all used expression fields
        /// </summary>
        public IEnumerable<string> FieldsRead => Expression.UsedData;

        /// <summary>
        /// Used expression
        /// </summary>
        public IEnumerable<Expression> Expressions => Expression.SingleEnumerable();

        internal AlertStage(BpProcess process, XElement element) : base(process, element)
        {
            Expression = new Expression(element.Element("alert").GetStringAttributeValue("expression"));
        }
    }

    /*
<stage stageid="ef114fcd-7ab4-4e58-9dce-56dee4ec4729" name="Alert1" type="Alert">
  <subsheetid>aa6b3806-af47-433c-99a0-d20cf36abe26</subsheetid>
  <loginhibit onsuccess="true" />
  <display x="15" y="360" />
  <onsuccess>de12a1d6-afe5-4d8a-9024-d49af4971059</onsuccess>
  <alert expression="[MyData]=&quot;&quot;" />
</stage>
     */
}
