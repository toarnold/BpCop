using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS028")]
    [ExportMetadata("Message", "The action '{0}' of the same VBO is called.")]
    [ExportMetadata("Description", "Do not call actions of the same VBO as action.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS028 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ActionStage>()
                .Where(a => a.ResourceObject == data.Asset.Name)
                .Select(s => data.BuildFinding(s, s.ResourceAction));
        }
    }
}
