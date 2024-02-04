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
    [ExportMetadata("RuleName", "SC004")]
    [ExportMetadata("Message", "Value of output non password field '{0}' is mapped from a password field")]
    [ExportMetadata("Description", "An output non password field shouldn't be mapped from a password field.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC004 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<EndStage>()
                .SelectMany(s => s.DataSinks.Where(w => w.DataType != DataType.Password && s.Page.FindDataField(w.Stage)?.DataType == DataType.Password)
                        .Select(a => new { Stage = s, a.Name }))
                .Select(s => data.BuildFinding(s.Stage, s.Name));
        }
    }
}
