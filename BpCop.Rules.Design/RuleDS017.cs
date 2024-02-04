using BpCop.Common;
using BpCop.Common.BpModel;
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
    [ExportMetadata("RuleName", "DS017")]
    [ExportMetadata("Message", "Global data '{0}' isn't in use")]
    [ExportMetadata("Description", "A global data field or collection is declared, but never used.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS017 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            var vars = data.Asset.Stages.OfType<IDataFields>().Where(d => !d.IsPrivate).SelectMany(k => k.DataFields.Select(s => s.Name));
            var accessedFields = data.Asset.Stages.OfType<IDataFieldsRead>().SelectMany(s => s.FieldsRead)
                .Union(data.Asset.Stages.OfType<IDataFieldsWrite>().SelectMany(s => s.FieldsWrite));
            var accessedCollection = accessedFields.Select(p => p.Split('.').First()).Distinct();

            return vars.Except(accessedFields).Except(accessedCollection)
                .Select(s => data.BuildFinding((Stage)data.Asset.Pages.First().Value.FindDataStage(s, false)!, s));
        }
    }
}
