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
    [ExportMetadata("RuleName", "SC002")]
    [ExportMetadata("Message", "Value of password field '{0}' written to a non-password field")]
    [ExportMetadata("Description", "The value of a password data field will be written to a non-password field.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC002 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ICalculation>()
                .SelectMany(s => s.Calculations.SelectMany(w => w.Expression.UsedData.Where(a => ((Stage)s).Page.FindDataField(a)?.DataType == DataType.Password && ((Stage)s).Page.FindDataField(w.Stage)?.DataType != DataType.Password)
                        .Select(a => new { Stage = (Stage)s, Name = a })))
                .Select(s => data.BuildFinding(s.Stage, s.Name));
        }
    }
}
