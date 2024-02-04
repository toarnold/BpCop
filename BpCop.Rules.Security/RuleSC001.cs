using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Security
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Security")]
    [ExportMetadata("RuleName", "SC001")]
    [ExportMetadata("Message", "Use property 'screen save capture' only temporarily")]
    [ExportMetadata("Description", "Exceptions screenshot can contain sensitive data, avoid long-time storing.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC001 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ExceptionStage>()
                .Where(w => w.SaveScreenCapture)
                .Select(s => data.BuildFinding(s));
        }
    }
}
