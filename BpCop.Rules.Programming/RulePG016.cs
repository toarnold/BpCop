using BpCop.Common;
using BpCop.Common.BpModel.Interfaces;
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
    [ExportMetadata("RuleName", "PG016")]
    [ExportMetadata("Message", "Expression is blank")]
    [ExportMetadata("Description", "Detects if a stage has an empty expression.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.None)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG016 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<IExpressions>()
                .Where(w => w is not ActionStage && w is not SubSheetStage && w is not ProcessStage  && (w is not ExceptionStage exp || !exp.UseCurrent))
                .Where(w => w.Expressions.Any(e => string.IsNullOrEmpty(e.Value)))
                .Select(s => data.BuildFinding((Stage)s));
        }
    }
}
