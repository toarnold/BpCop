using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Stages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace BpCop.Common
{
    public static class JustificationFactory
    {
        private static readonly Regex JustifyExpression = new(@"\[(?<RuleName>[A-Z]{2}\d{3})\s*(?<StageName>;.+?[;\]])?(?<Justification>.*?)\]", RegexOptions.Compiled);

        internal static IEnumerable<Justification> BuildFromNote(GenericStage note)
        {
            return JustifyExpression.Matches(note.Narrative)
                .Cast<Match>()
                .Select(m => new Justification
                {
                    AssetName = note.Process.Name,
                    AssetType = note.Process.AssetType.ToString(),
                    PageName = note.Page.Name,
                    RuleName = m.Groups["RuleName"].Value,
                    StageName = m.Groups["StageName"].Value.Trim(';', ' '),
                    Message = m.Groups["Justification"].Value.Trim(),
                    Level = note.StageName.IndexOf("global", StringComparison.OrdinalIgnoreCase) >= 0 ? JustificationLevel.Global : string.IsNullOrEmpty(m.Groups["StageName"].Value.Trim(';', ' ')) ? JustificationLevel.Page : JustificationLevel.Stage
                });
        }

        internal static IEnumerable<Justification> BuildFromAppmodelElement(BpProcess process, XElement element)
        {
            return JustifyExpression.Matches(element.Element("description").Value)
                .Cast<Match>()
                .Select(m => new Justification
                {
                    AssetName = process.Name,
                    AssetType = process.AssetType.ToString(),
                    PageName = string.Empty,
                    RuleName = m.Groups["RuleName"].Value,
                    StageName = element.BuildPath(),
                    Message = m.Groups["Justification"].Value.Trim(),
                    Level = JustificationLevel.AppModel
                });
        }
    }
}
