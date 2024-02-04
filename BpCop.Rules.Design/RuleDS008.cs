using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS008")]
    [ExportMetadata("Message", "A published process should expose at lease one data variable to the control room")]
    [ExportMetadata("Description", "A published process should expose at lease one data variable to the control room.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global)]
    [ExportMetadata("AppliesTo", AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS008 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (data.Asset.IsPublished && data.Asset.Stages.OfType<DataStage>().All(s => s.Exposure != "Session"))
            {
                return data.BuildFinding().SingleEnumerable();
            }
            else
            {
                return Enumerable.Empty<CheckResult>();
            }
        }
    }
}
