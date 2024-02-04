using BpCop.Common;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS025")]
    [ExportMetadata("Message", "The Appmodel field is not used.")]
    [ExportMetadata("Description", "AppModel field is defined but never used in a read, write, navigate or wait stage.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.AppModel)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS025 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (!data.Asset.HasAppModel)
            {
                return Enumerable.Empty<CheckResult>();
            }
            var elementIds = data.Asset.AppModel.Descendants("element").Select(e => Guid.Parse(e.Element("id").Value));
            var usedFields = data.Asset.Stages.OfType<IAppModelFields>().SelectMany(m => m.AppModelFields).Distinct();

            return elementIds.Except(usedFields).Select(s => data.BuildFindingFromAppModel(data.Asset.AppModel.BuildPath(s)));
        }
    }
}
