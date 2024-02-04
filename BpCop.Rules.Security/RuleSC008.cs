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
    [ExportMetadata("RuleName", "SC008")]
    [ExportMetadata("Message", "Value of output password field '{0}' is mapped to the non password field '{1}'")]
    [ExportMetadata("Description", "An output password field shouldn't be mapped to a non password field.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.None)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC008 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<IDataSinks>()
                .SelectMany(m => m.DataSinks
                    .Where(s => s.DataType == DataType.Password && ((Stage)m).Page.FindDataField(s.Stage)?.DataType != DataType.Password)
                    .Select(s => data.BuildFinding((Stage)m, s.Name, s.Stage))
                );
        }
    }
}
