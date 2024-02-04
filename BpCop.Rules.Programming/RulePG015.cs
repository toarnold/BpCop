using BpCop.Common;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG015")]
    [ExportMetadata("Message", "Not all exits of this stage are connected")]
    [ExportMetadata("Description", "Connect all exits of a stage.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.None)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG015 : ICheckRule
    {
        private static readonly string[] ExcludeStageType = new[] { StageType.End, StageType.Note, StageType.Data, StageType.Collection, StageType.SubSheetInfo, StageType.Block, StageType.Exception, StageType.ProcessInfo };

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
               .Where(w => !ExcludeStageType.Contains(w.StageType) && w.NextStages.Any(a => a == Guid.Empty))
               .Select(s => data.BuildFinding(s));
        }
    }
}
