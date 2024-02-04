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
    [ExportMetadata("RuleName", "DS021")]
    [ExportMetadata("Message", "Application Model field has {0} matching attributes. Allowed are {1}.")]
    [ExportMetadata("Description", "An Application Model field should have not more than 'MaxMatches' matching attributes.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.AppModel)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"MaxMatches\":6}")]
    internal sealed class RuleDS021 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (!data.Asset.HasAppModel)
            {
                return Enumerable.Empty<CheckResult>();
            }
            return data.Asset.AppModel.XPathSelectElements($".//element[count(attributes/attribute[@inuse='True'])>{data.Parameter.MaxMatches}]")
                .Select(s => data.BuildFindingFromAppModel(s.BuildPath(), s.XPathSelectElements("attributes/attribute[@inuse='True']").Count(), (int)data.Parameter.MaxMatches));
        }
    }
}
