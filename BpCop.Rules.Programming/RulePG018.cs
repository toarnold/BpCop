using BpCop.Common;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG018")]
    [ExportMetadata("Message", "Note contains match '{0}'")]
    [ExportMetadata("Description", "Checks if a note contains specific keywords or expressions.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"Expressions\":[\"(?i)todo[^A-Z]\"]}")]
    internal sealed class RulePG018 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            var expressions = ((IEnumerable<string>)data.Parameter.Expressions.ToObject<IList<string>>())?.Select(x => new Regex(x)) ?? Enumerable.Empty<Regex>();
            return data.Asset.Stages.Where(s => s.StageType == StageType.Note)
                .SelectMany(n => expressions.Select(x => x.Match(n.Narrative)).Where(w => w.Success).Select(r => data.BuildFinding(n, r.Value)));
        }
    }
}
