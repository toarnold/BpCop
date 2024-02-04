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
    [ExportMetadata("RuleName", "DS016")]
    [ExportMetadata("Message", "Private data '{0}' isn't in use")]
    [ExportMetadata("Description", "A private data field or collection is declared, but never used.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS016 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Pages.Values.SelectMany(p =>
            {
                var vars = data.Asset.Stages.OfType<IDataFields>().Where(d => d.IsPrivate && ((Stage)d).Page == p).SelectMany(k => k.DataFields.Select(s => s.Name));
                var accessedFields = data.Asset.Stages.OfType<IDataFieldsRead>().Where(d => ((Stage)d).Page == p).SelectMany(k => k.FieldsRead)
                    .Union(data.Asset.Stages.OfType<IDataFieldsWrite>().Where(d => ((Stage)d).Page == p).SelectMany(k => k.FieldsWrite));
                var accessedCollection = accessedFields.Select(px => px.Split('.').First()).Distinct();

                return vars.Except(accessedFields).Except(accessedCollection)
                    .Select(s => data.BuildFinding((Stage)p.FindDataStage(s, true)!, s));
            });
        }
    }
}
