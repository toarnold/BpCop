using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS030")]
    [ExportMetadata("Message", "'Preserve Exception' stage without previous recover stage found.")]
    [ExportMetadata("Description", "Use 'Preserve Exception' stage only after a recover stage.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO | AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS030 : ICheckRule
    {
        public static bool IsAfterRecover(Stage? stage, ICollection<Guid> cyclicList)
        {
            if (stage is not null && (cyclicList.Contains(stage.StageId) || stage is RecoverStage)) // Cycles or recovers are ok.
            {
                return true;
            }
            if (stage is null || stage.StageType == StageType.Resume || !stage.PreviousStages.Any()) // Resume or missing prev. terminates
            {
                return false;
            }
            cyclicList.Add(stage.StageId);
            // else check all callers
            return stage.PreviousStages.All(n => IsAfterRecover(stage.Process.GetStage(n), cyclicList));
        }

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ExceptionStage>()
                .Where(w => w.UseCurrent && !IsAfterRecover(w, new HashSet<Guid>()))
                .Select(s => data.BuildFinding(s));
        }
    }
}
