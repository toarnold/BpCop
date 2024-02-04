using BpCop.Common;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.XPath;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS026")]
    [ExportMetadata("Message", "The AppModel group is empty.")]
    [ExportMetadata("Description", "AppModel group is defined but empty.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.AppModel)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS026 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (!data.Asset.HasAppModel)
            {
                return Enumerable.Empty<CheckResult>();
            }
            return data.Asset.AppModel.XPathSelectElements(".//group[not(element or group)]")
                .Select(s => data.BuildFindingFromAppModel(s.BuildPath()));
        }
    }
}
