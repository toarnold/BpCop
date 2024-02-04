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
    [ExportMetadata("RuleName", "DS014")]
    [ExportMetadata("Message", "No action output parameter mapped")]
    [ExportMetadata("Description", "Map at least one output paramter of a called action.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS014 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ActionStage>()
                .Where(w => w.DataSinks.Any() && w.DataSinks.All(a => string.IsNullOrWhiteSpace(a.Stage)))
                .Select(s => data.BuildFinding(s));
        }
    }
}
