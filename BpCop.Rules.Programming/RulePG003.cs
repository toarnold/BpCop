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
    [ExportMetadata("RuleName", "PG003")]
    [ExportMetadata("Message", "Multiple use of Exception '{0}'")]
    [ExportMetadata("Description", "Check unique Exception/Messages per VBO/process")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG003 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ExceptionStage>()
                .Where(e => !e.UseCurrent)
                .GroupBy(g => new { g.ExceptionType, Detail = g.Expression.Value })
                .Where(w => w.Count() > 1)
                .SelectMany(s => s.Select(g => data.BuildFinding(g, $"{s.Key.ExceptionType} - {s.Key.Detail}")));
        }
    }
}
