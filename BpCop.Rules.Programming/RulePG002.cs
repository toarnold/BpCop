using BpCop.Common;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG002")]
    [ExportMetadata("Message", "'Attach' action called without specifying parameters")]
    [ExportMetadata("Description", "An AppModel calls 'Attach' action without specifying parameters. This can lead to errors.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG002 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<NavigateStage>()
                .Where(w => w.Steps.Any(a => a.Action.Id == "AttachApplication" && a.Action.Input.All(x => string.IsNullOrWhiteSpace(x.Value.Value))))
                .Select(s => data.BuildFinding(s));
        }
    }
}
