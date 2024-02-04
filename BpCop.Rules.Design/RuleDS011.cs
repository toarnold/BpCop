using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS011")]
    [ExportMetadata("Message", "Input field '{0}' should have a description")]
    [ExportMetadata("Description", "An input field on a public available action should have a description")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS011 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<StartStage>()
                .Where(w => w.Page.IsPublished && !w.Page.IsMainPage)
                .SelectMany(w => w.DataSinks.Where(s => string.IsNullOrWhiteSpace(s.Narrative))
                    .Select(s => data.BuildFinding(w, s.Name)));
        }
    }
}
