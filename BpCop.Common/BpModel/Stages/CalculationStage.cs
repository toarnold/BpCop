using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class CalculationStage : Stage, IDataFieldsRead, IDataFieldsWrite, ICalculation, IExpressions
    {
        public Calculation Calculation { get; init; }
        public IEnumerable<string> FieldsRead => Calculation.Expression.UsedData;
        public IEnumerable<string> FieldsWrite => Calculation.Stage.SingleEnumerable();
        public IList<Calculation> Calculations => Calculation.SingleEnumerable().ToList();
        public IEnumerable<Expression> Expressions => Calculation.Expression.SingleEnumerable();

        internal CalculationStage(BpProcess process, XElement element) : base(process, element)
        {
            Calculation = new Calculation(element.Element("calculation"));
        }

        /*
<stage stageid="7ce15790-8e54-4b64-88e0-3abcccb91603" name="Calc1" type="Calculation">
  <subsheetid>aa6b3806-af47-433c-99a0-d20cf36abe26</subsheetid>
  <loginhibit onsuccess="true" />
  <display x="15" y="165" />
  <onsuccess>3c4b6c6d-511a-48c9-aebf-03ae22c93fb7</onsuccess>
  <calculation expression="[MyData]&amp;&quot;-&quot;" stage="MyData" />
</stage>
        */
    }
}
