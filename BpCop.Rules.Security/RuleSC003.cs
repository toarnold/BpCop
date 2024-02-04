using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Security
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Security")]
    [ExportMetadata("RuleName", "SC003")]
    [ExportMetadata("Message", "Value of password field '{0}' is used in an exception/alert stage")]
    [ExportMetadata("Description", "The value of a password data field should not be used in an exception or alert.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC003 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
                .Where(w => w.StageType == StageType.Exception || w.StageType == StageType.Alert)
                .Cast<IDataFieldsRead>()
                .SelectMany(w => w.FieldsRead.Select(n => ((Stage)w).Page.FindDataField(n))
                                        .Where(a => a?.DataType == DataType.Password)
                                        .Select(a => new { Stage = (Stage)w, a!.Name }))
                .Select(s => data.BuildFinding(s.Stage, s.Name));
        }
    }
}
