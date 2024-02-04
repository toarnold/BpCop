﻿using BpCop.Common;
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
    [ExportMetadata("RuleName", "PG006")]
    [ExportMetadata("Message", "Decision logging is disabled")]
    [ExportMetadata("Description", "Consider to activate at least 'Errors Only' logging on decision stages.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG006 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (data.Asset.HasAppModel)
            {
                return Enumerable.Empty<CheckResult>();
            }
            return data.Asset.Stages.OfType<DecisionStage>()
                .Where(w => w.Logging== "Disabled")
                .Select(s => data.BuildFinding(s));
        }
    }
}
