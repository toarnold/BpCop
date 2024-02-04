using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class CodeStage : Stage, IDataFieldsRead, IDataFieldsWrite, IDataSinks, IDataSources
    {
        public IEnumerable<DataSource> DataSources { get; init; }
        public IEnumerable<DataSink> DataSinks { get; init; }
        public IEnumerable<string> FieldsRead => DataSources.SelectMany(s => s.Expression.UsedData).Distinct();
        public IEnumerable<string> FieldsWrite => DataSinks.Select(s => s.Stage).Where(w => !string.IsNullOrEmpty(w)).Distinct();

        internal CodeStage(BpProcess process, XElement element) : base(process, element)
        {
            DataSources = DataSource.ToEnumerable(element.Element("inputs")?.Elements("input"));
            DataSinks = DataSink.ToEnumerable(element.Element("outputs")?.Elements("output"));
        }

        /*
<stage stageid="cd19a0ac-111b-417d-93ae-5701fecf0274" name="Average" type="Code">
  <subsheetid>abad44af-1e40-40d0-a823-0c2450d622bc</subsheetid>
  <loginhibit />
  <display x="15" y="75" />
  <inputs>
    <input type="collection" name="source" expr="[Source]" />
    <input type="text" name="predicate" expr="[Predicate]" />
    <input type="collection" name="parameter" expr="[Parameter]" />
  </inputs>
  <outputs>
    <output type="number" name="avg" stage="Avg" />
  </outputs>
  <onsuccess>eba4701c-e9d0-412d-a1c4-78a86802d5b8</onsuccess>
  <code><![CDATA[var result = InvokeLinqMethod("Average", predicate, source, parameter);
avg = (decimal)result.Rows[0][0];]]></code>
</stage>
        */
    }
}
