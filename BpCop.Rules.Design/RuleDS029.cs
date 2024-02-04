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
    [ExportMetadata("RuleName", "DS029")]
    [ExportMetadata("Message", "No resume/preserve stage for recover detected.")]
    [ExportMetadata("Description", "Every 'Recover' stage should end in a 'Resume' or 'Preserve Exception' stage.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO | AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS029 : ICheckRule
    {
        private static bool TerminatesCorrectly(Stage? stage, ICollection<Guid> cyclicList)
        {
            if (stage is null || cyclicList.Contains(stage.StageId)) // prevent endless recursion
            {
                return false;
            }
            if (stage.StageType == StageType.Resume || ((stage is ExceptionStage exp) && exp.UseCurrent))
            {
                return true;
            }
            cyclicList.Add(stage.StageId);
            return stage.NextStages.All(n => TerminatesCorrectly(stage.Process.GetStage(n), cyclicList));
        }

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<RecoverStage>()
                .Where(w => !TerminatesCorrectly(w, new HashSet<Guid>()))
                .Select(s => data.BuildFinding(s));
        }
    }
}
