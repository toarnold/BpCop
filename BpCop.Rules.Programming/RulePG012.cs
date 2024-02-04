using BpCop.Common;
using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG012")]
    [ExportMetadata("Message", "Published action didn't call 'Attach' page")]
    [ExportMetadata("Description", "Published action in an object with an AppModel should call 'Attach' page")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page)]
    [ExportMetadata("AppliesTo", AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"IgnorePage\":[\"Initialise\",\"Clean Up\"]}")]
    internal sealed class RulePG012 : ICheckRule
    {
        private static bool HasAttach(Page page, ICollection<Guid> cyclicList)
        {
            if (cyclicList.Contains(page.SubSheetId)) // prevent endless recursion
            {
                return false;
            }
            if (page.Stages.Values.OfType<NavigateStage>().Any(w => w.Steps.Any(a => a.Action.Id == "AttachApplication")))
            {
                return true;
            }
            cyclicList.Add(page.SubSheetId);
            var calls = page.Stages.Values.OfType<SubSheetStage>();
            return calls.Any(c => HasAttach(page.Process.Pages[c.TargetSubSheetId], cyclicList));
        }

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            if (data.Asset.HasAppModel)
            {
                return data.Asset.Pages.Values.Where(w => w.IsPublished 
                    && !data.Parameter.IgnorePage.ToObject<IList<string>>().Contains(w.Name)
                    && !HasAttach(w, new HashSet<Guid>()))
                    .Select(s => data.BuildFinding(s));
            }
            else
            {
                return Enumerable.Empty<CheckResult>();
            }
        }
    }
}
