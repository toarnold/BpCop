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
    [ExportMetadata("RuleName", "DS022")]
    [ExportMetadata("Message", "The page is defined but never called.")]
    [ExportMetadata("Description", "A process page should called at least once.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS022 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            // Left outer join pages with SubSheetStages
            return data.Asset.Pages.GroupJoin(
                data.Asset.Stages.OfType<SubSheetStage>(),
                o => o.Key, i => i.TargetSubSheetId, (o, inners) => new { Page = o.Value, Count = inners.Count() })
                .Where(w => w.Count == 0 && !w.Page.IsMainPage)
                .Select(s => data.BuildFinding(s.Page));
        }
    }
}
