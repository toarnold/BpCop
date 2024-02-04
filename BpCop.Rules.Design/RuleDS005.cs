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
    [ExportMetadata("RuleName", "DS005")]
    [ExportMetadata("Message", "Output field '{0}' should have a description")]
    [ExportMetadata("Description", "An output field on process 'Main Page' should have a description")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.Process)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS005 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
                .Where(w => w.StageType == StageType.End && w.Page.IsMainPage)
                .Cast<EndStage>()
                .SelectMany(w => w.DataSinks.Where(s => string.IsNullOrWhiteSpace(s.Narrative))
                                    .Select(s => data.BuildFinding(w, s.Name)));
        }
    }
}
