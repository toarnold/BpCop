using BpCop.Common;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS003")]
    [ExportMetadata("Message", "Process page should have a description")]
    [ExportMetadata("Description", "A process page should have a description.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS003 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
                .Where(w => w.StageType == StageType.SubSheetInfo && string.IsNullOrWhiteSpace(w.Narrative))
                .Select(s => data.BuildFinding(s.Page));
        }
    }
}
