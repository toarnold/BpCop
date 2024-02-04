using BpCop.Common;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS024")]
    [ExportMetadata("Message", "The stage is not referenced by any other stage.")]
    [ExportMetadata("Description", "An asset should not have unreachable code.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS024 : ICheckRule
    {
        private static readonly string[] ExcludeStageType = new[] { StageType.ProcessInfo, StageType.Note, StageType.Start, StageType.Data, StageType.Collection, StageType.SubSheetInfo, StageType.Recover, StageType.Block };
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.Where(w => !ExcludeStageType.Contains(w.StageType) && !w.PreviousStages.Any())
                .Select(s => data.BuildFinding(s));
        }
    }
}
