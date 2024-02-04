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
    [ExportMetadata("RuleName", "DS015")]
    [ExportMetadata("Message", "No page output parameter mapped")]
    [ExportMetadata("Description", "Map at least one output paramter of a called page.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS015 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            var calls1 = data.Asset.Stages.OfType<SubSheetStage>()
                .Where(w => w.DataSinks.Any() && w.DataSinks.All(a => string.IsNullOrWhiteSpace(a.Stage)))
                .Select(s => data.BuildFinding(s));

            // Bug in BP: A fresh and not edited page reference has no parameters (even if it should) - Check called page end instead.
            var calls2 = data.Asset.Stages.OfType<SubSheetStage>()
                .Where(w => !w.DataSinks.Any() && data.Asset.Stages.OfType<EndStage>().Any(x => x.SubSheetId == w.TargetSubSheetId && x.DataSinks.Any()))
                .Select(s => data.BuildFinding(s));

            return calls1.Concat(calls2);
        }
    }
}
