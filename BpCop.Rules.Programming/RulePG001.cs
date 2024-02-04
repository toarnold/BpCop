using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet","Programming")]
    [ExportMetadata("RuleName", "PG001")]
    [ExportMetadata("Message", "VBO did not use an Application Model, consider to use 'Background' mode.")]
    [ExportMetadata("Description", "Checks if a VBOs should run in background mode, if not using an Application Model")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG001 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (!data.Asset.HasAppModel && data.Asset.Runmode != "Background")
            {
                return data.BuildFinding().SingleEnumerable();
            }
            return Enumerable.Empty<CheckResult>();
        }
    }
}
