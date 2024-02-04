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
    [ExportMetadata("RuleName", "DS027")]
    [ExportMetadata("Message", "The published action '{0}' of the same VBO is called.")]
    [ExportMetadata("Description", "Using published actions of the same VBO is a design flaw.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS027 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<SubSheetStage>()
                .Where(w => data.Asset.Pages[w.TargetSubSheetId].IsPublished)
                .Select(s => data.BuildFinding(s, data.Asset.Pages[s.TargetSubSheetId].Name));
        }
    }
}
