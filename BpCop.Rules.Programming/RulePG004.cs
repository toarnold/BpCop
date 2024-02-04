using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG004")]
    [ExportMetadata("Message", "Acquire lock without provided timeout")]
    [ExportMetadata("Description", "Provide a timeout when acquire a lock to prevent a freeze.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG004 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ActionStage>()
                .Where(w => w.ResourceAction == "Acquire Lock" && w.DataSources.Any(a => a.Name == "Timeout" && string.IsNullOrWhiteSpace(a.Expression.Value)))
                .Select(s => data.BuildFinding(s));
        }
    }
}
