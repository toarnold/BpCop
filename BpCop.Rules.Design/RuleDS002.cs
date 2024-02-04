using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS002")]
    [ExportMetadata("Message", "The '{0}' page should have a description")]
    [ExportMetadata("Description", "The process 'Main Page'/object 'Initialise' page should have a description.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS002 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (string.IsNullOrWhiteSpace(data.Asset.Narrative))
            {
                return data.BuildFinding(data.Asset.Pages[System.Guid.Empty], data.Asset.Pages[System.Guid.Empty].Name).SingleEnumerable();
            }
            return Enumerable.Empty<CheckResult>();
        }
    }
}
