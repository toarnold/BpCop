using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.XPath;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG005")]
    [ExportMetadata("Message", "'Activate Window' without 'Wait Activated'")]
    [ExportMetadata("Description", "Activate windows only in combination with a wait expression on 'activated'")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG005 : ICheckRule
    {
        private static bool TraceRoute(IDictionary<Guid, Stage> stages, Guid nextId)
        {
            if (nextId == Guid.Empty || !stages.TryGetValue(nextId, out Stage node))
            {
                return false;
            }

            switch (node.StageType)
            {
                case StageType.Anchor:
                case StageType.Note:
                case StageType.Calculation:
                    break;
                case StageType.WaitStart:
                    return (bool)node.StageElement.XPathEvaluate("choices/choice/condition/id='CheckWindowActive'");
                default:
                    return false;
            }

            return TraceRoute(stages, node.OnSuccess);
        }

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<NavigateStage>()
                .Where(w => w.Steps.Any(a => a.Action.Id == "ActivateApp")
                        && !TraceRoute(w.Page.Stages, w.OnSuccess))
                .Select(s => data.BuildFinding(s));
        }
    }
}
