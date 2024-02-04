using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS010")]
    [ExportMetadata("Message", "Object page should have a description")]
    [ExportMetadata("Description", "A published action should have a description.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS010 : ICheckRule
    {
        static readonly string[] DS010Exceptions = new[] { StageType.SubSheetInfo, StageType.Anchor, StageType.Note, StageType.Start, StageType.End };

        // Test if page is "CleanUp" and report if has content
        private static bool IsFilledCleanupOrOther(Page page, IEnumerable<Stage> stages) => page.PageType != PageType.CleanUp || stages.Select(s => s.StageType).Distinct().Any(w => !DS010Exceptions.Contains(w));
        
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
                .Where(w => w.Page.IsPublished && w.StageType == StageType.SubSheetInfo && !w.Page.IsMainPage && IsFilledCleanupOrOther(w.Page, w.Process.Stages.Where(x => x.SubSheetId == w.Page.SubSheetId)) && string.IsNullOrWhiteSpace(w.Narrative))
                .Select(s => data.BuildFinding(s.Page));
        }
    }
}
