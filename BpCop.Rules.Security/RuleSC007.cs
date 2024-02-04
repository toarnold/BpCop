using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Security
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Security")]
    [ExportMetadata("RuleName", "SC007")]
    [ExportMetadata("Message", "Never use action 'Invoke Javascript Function'")]
    [ExportMetadata("Description", "Deprecated: The browser action 'Invoke Javascript Function' is not supported after 06/2023 anymore.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC007 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            // BrowserPlugin -> WebInvokeJavascript
            // IE Mode -> HTMLInvokeJavascriptMethod
            return data.Asset.Stages.OfType<NavigateStage>()
                .Where(s => s.Steps.Any(stp => stp.Action.Id == "WebInvokeJavascript" || stp.Action.Id == "HTMLInvokeJavascriptMethod"))
                .Select(s => data.BuildFinding(s));
        }
    }
}
