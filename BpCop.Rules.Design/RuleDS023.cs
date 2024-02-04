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
    [ExportMetadata("RuleName", "DS023")]
    [ExportMetadata("Message", "The not published page is defined but never called.")]
    [ExportMetadata("Description", "A not published object page should called at least once.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS023 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            // Left outer join pages with SubSheetStages
            return data.Asset.Pages.Where(p => !p.Value.IsPublished).GroupJoin(
                data.Asset.Stages.OfType<SubSheetStage>(),
                o => o.Key, i => i.TargetSubSheetId, (o, inners) => new { Page = o.Value, Count = inners.Count() })
                .Where(w => w.Count == 0)
                .Select(s => data.BuildFinding(s.Page));
        }
    }
}
