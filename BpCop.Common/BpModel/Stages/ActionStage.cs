using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class ActionStage : Stage, IDataFieldsRead, IDataFieldsWrite, IExpressions, IDataSinks, IDataSources
    {
        private readonly Regex StringMatch = new(@"^\s*\""(?<Content>.+?)\""\s*$", RegexOptions.Compiled);
        private readonly XElement _actionElement;

        /// <summary>
        /// Name of the referenced VBO
        /// </summary>
        public string ResourceObject { get; init; }

        /// <summary>
        /// Name of the referenced action
        /// </summary>
        public string ResourceAction { get; init; }

        /// <summary>
        /// Enumerates all inputs
        /// </summary>
        public IEnumerable<DataSource> DataSources { get; init; }

        /// <summary>
        /// Enumerates all outputs
        /// </summary>
        public IEnumerable<DataSink> DataSinks { get; init; }

        /// <summary>
        /// Enumerates all fields used as input arguments
        /// </summary>
        public IEnumerable<string> FieldsRead => DataSources.SelectMany(s => s.Expression.UsedData).Union(BuiltinVboReads());

        /// <summary>
        /// Enumerates all fields mapped as output field
        /// </summary>
        public IEnumerable<string> FieldsWrite => DataSinks.Select(s => s.Stage).Where(w => !string.IsNullOrEmpty(w)).Union(BuiltinVboWrites());

        /// <summary>
        /// Enumerates all input expressions
        /// </summary>
        public IEnumerable<Expression> Expressions => DataSources.Select(d => d.Expression);

        internal ActionStage(BpProcess process, XElement element) : base(process, element)
        {
            _actionElement = element.Element("resource");
            ResourceObject = _actionElement.GetStringAttributeValue("object");
            ResourceAction = _actionElement.GetStringAttributeValue("action");
            DataSources = DataSource.ToEnumerable(element.Element("inputs")?.Elements("input"));
            DataSinks = DataSink.ToEnumerable(StageElement.Element("outputs")?.Elements("output"));
        }

        private IEnumerable<string> TryExtractVarNameFromSource(string sourceName)
        {
            var colName = DataSources.FirstOrDefault(f => f.Name == sourceName);
            if (colName is not null)
            {
                var match = StringMatch.Match(colName.Expression.Parts.FirstOrDefault() ?? string.Empty);
                if (match.Success)
                {
                    yield return match.Groups["Content"].Value;
                }
            }
        }

        private IEnumerable<string> BuiltinVboReads()
        {
            if (ResourceObject == "Blueprism.AutomateProcessCore.clsCollectionActions")
            {
                switch (ResourceAction)
                {
                    case "Copy Rows":
                    case "Count Rows":
                    case "Count Columns":
                        return TryExtractVarNameFromSource("Collection Name");
                }
            }
            return Enumerable.Empty<string>();
        }

        private IEnumerable<string> BuiltinVboWrites()
        {
            if (ResourceObject == "Blueprism.AutomateProcessCore.clsCollectionActions")
            {
                switch (ResourceAction)
                {
                    case "Add Row":
                    case "Remove All Rows":
                    case "Remove Row":
                        return TryExtractVarNameFromSource("Collection Name");
                }
            }
            return Enumerable.Empty<string>();
        }

        /*
<stage stageid="81d09a4d-b069-4968-b6ae-1dec8a25313e" name="Get Files" type="Action">
  <subsheetid>ddf77b7a-2aa2-4d3e-afb9-7dcdc9334925</subsheetid>
  <loginhibit onsuccess="true" />
  <display x="15" y="-15" />
  <inputs>
    <input type="text" name="Folder" friendlyname="Folder" narrative="The folder in which to look for files" expr="" />
    <input type="text" name="Patterns CSV" friendlyname="Patterns CSV" narrative="The comma seperated list of wildcard patterns" expr="" />
  </inputs>
  <outputs>
    <output type="flag" name="Success" friendlyname="Success" narrative="True if successful" stage="Success" />
    <output type="text" name="Message" friendlyname="Message" narrative="A message if unsuccessful" stage="" />
    <output type="collection" name="Files" friendlyname="Files" narrative="The collection of files found" stage="" />
  </outputs>
  <onsuccess>9469a7bb-d41b-4b90-b3a1-fd1413f4e1da</onsuccess>
  <resource object="Utility - File Management" action="Get Files" />
</stage>


 -- SPECIAL: resource Builtin ---

  <stage stageid="fe8ce08c-0859-46bf-acbe-f616a921fb37" name="Collections::Count Rows" type="Action">
    <subsheetid>30f2db60-5962-4e71-a2dd-49eaebc6ee7e</subsheetid>
    <loginhibit onsuccess="true" />
    <display x="15" y="75" />
    <inputs>
      <input type="text" name="Collection Name" friendlyname="Collection Name" narrative="The name of the collection to act upon" expr="&quot;Results&quot;" />
    </inputs>
    <outputs>
      <output type="number" name="Count" friendlyname="Count" narrative="The number of rows counted in the collection" stage="Count" />
    </outputs>
    <onsuccess>cf7db194-c344-4b63-aacc-53e08e751452</onsuccess>
    <resource object="Blueprism.AutomateProcessCore.clsCollectionActions" action="Count Rows" />
  </stage>

        */
    }
}
