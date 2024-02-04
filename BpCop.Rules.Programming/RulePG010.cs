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
    [ExportMetadata("RuleName", "PG010")]
    [ExportMetadata("Message", "'Get Next Item' logging is disabled")]
    [ExportMetadata("Description", "Consider to activate at least 'Errors Only' logging on 'Get Next Item' stages.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG010 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ActionStage>()
                .Where(w => w.Logging == "Disabled" && w.ResourceAction== "Get Next Item")
                .Select(s => data.BuildFinding(s));
        }
    }
}
