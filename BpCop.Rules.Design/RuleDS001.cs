using BpCop.Common;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS001")]
    [ExportMetadata("Message", "{1} stages found. Consider a redesign with not more than {0} stages.")]
    [ExportMetadata("Description", "A action/page should not contain more than 'MaxStages' stages.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"MaxStages\":20}")]
    internal sealed class RuleDS001 : ICheckRule
    {
        static readonly string[] DS001Exceptions = [ StageType.ProcessInfo, StageType.SubSheetInfo, StageType.Block,
            StageType.Anchor, StageType.Data, StageType.Collection, StageType.Note, StageType.Start, StageType.End ];

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
                .Where(w => !DS001Exceptions.Contains(w.StageType))
                .GroupBy(g => g.SubSheetId)
                .Where(g => g.Count() > (int)data.Parameter.MaxStages)
                .Select(s => data.BuildFinding(data.Asset.Pages[s.Key], (int)data.Parameter.MaxStages, s.Count()));
        }
    }
}
