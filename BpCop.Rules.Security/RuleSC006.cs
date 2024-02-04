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
    [ExportMetadata("RuleName", "SC006")]
    [ExportMetadata("Message", "Never use action 'Insert Javascript Fragment'")]
    [ExportMetadata("Description", "Deprecated: The browser action 'Insert Javascript Fragment' is not supported after 06/2023 anymore.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleSC006 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            // BrowserPlugin -> WebInjectJavascript
            // IE Mode -> HTMLInsertJavascriptFragment
            return data.Asset.Stages.OfType<NavigateStage>()
                .Where(s => s.Steps.Any(stp => stp.Action.Id == "WebInjectJavascript" || stp.Action.Id == "HTMLInsertJavascriptFragment"))
                .Select(s => data.BuildFinding(s));
        }
    }
}
