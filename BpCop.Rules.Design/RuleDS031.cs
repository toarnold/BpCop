using BpCop.Common.BpModel.Stages;
using BpCop.Common;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using BpCop.Common.BpModel.Types;

namespace BpCop.Rules.Design
{
#pragma warning disable CA1812 // record is used by JsonSerializer
    internal sealed record DeprecatedAsset(string ObjectName, IEnumerable<string> ActionNames);
#pragma warning restore CA1812

    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS031")]
    [ExportMetadata("Message", "Deprecated action '{0}' of object '{1}' used.")]
    [ExportMetadata("Description", "Check usages of deprecated objects/actions.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"Deprecated\":[]}")] // Array of objects { "ObjectName": "VBO Name", "ActionNames": ["Do it .."]}, ActionNames is optional to deprecate whole objects
    internal sealed class RuleDS031 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            var deprecated = (IList<DeprecatedAsset>)data.Parameter.Deprecated.ToObject<List<DeprecatedAsset>>();

            return data.Asset.Stages.OfType<ActionStage>()
                .Join(deprecated, s => s.ResourceObject, o => o.ObjectName, (x, y) => y.ActionNames is null || !y.ActionNames.Any() || y.ActionNames.Contains(x.ResourceAction) ? x : null)
                .Where(w => w is not null)
                .Select(s => data.BuildFinding(s!, s!.ResourceAction, s!.ResourceObject));
        }
    }
}
