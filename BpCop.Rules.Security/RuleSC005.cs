using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Security
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Security")]
    [ExportMetadata("RuleName", "SC005")]
    [ExportMetadata("Message", "Value of input password field '{0}' is mapped to a non-password field")]
    [ExportMetadata("Description", "Never map a password input parameter to a non-password field.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC005 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<StartStage>()
                .SelectMany(s => s.DataSinks.Where(w => w.DataType == DataType.Password && s.Page.FindDataField(w.Name)?.DataType != DataType.Password)
                        .Select(a => new { Stage = s, a.Name }))
                .Select(s => data.BuildFinding(s.Stage, s.Name));
        }
    }
}
