using BpCop.Common.BpModel.Types;
using BpCop.Common;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BpCop.Common.BpModel.Stages;
using System;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG017")]
    [ExportMetadata("Message", "Choice expression is not unique")]
    [ExportMetadata("Description", "A choice stage should have unqiue expressions.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.None)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG017 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ChoiceStartStage>()
                .Where(w => w.Expressions.Select(e => e.Value.Trim()).Distinct().Count() != w.Expressions.Count())
                .Select(s => data.BuildFinding(s));
        }
    }
}
