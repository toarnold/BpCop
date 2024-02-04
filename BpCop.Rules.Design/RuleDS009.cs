using BpCop.Common;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS009")]
    [ExportMetadata("Message", "Consider to move global data '{0}' to '{1}' page")]
    [ExportMetadata("Description", "Global available data on Initialise/Main Page only.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"AllowedPages\":[\"Initialise\",\"Main Page\"]}")]
    internal sealed class RuleDS009 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            IList<string> allowedPages = data.Parameter.AllowedPages.ToObject<IList<string>>();
            return data.Asset.Stages.OfType<IDataFields>()
                .Where(w => !w.IsPrivate && !allowedPages.Contains(((Stage)w).Page.Name))
                .Select(s => data.BuildFinding((Stage)s, ((Stage)s).StageName, 
                    string.Join("/", (IEnumerable<string>)data.Parameter.AllowedPages.ToObject<IList<string>>())));
        }
    }
}
