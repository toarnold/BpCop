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
    [ExportMetadata("RuleName", "DS018")]
    [ExportMetadata("Message", "Private data '{0}' is written only")]
    [ExportMetadata("Description", "A private data field or collection will be written, but never read.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS018 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Pages.Values.SelectMany(p =>
            {
                var dataWritten = data.Asset.Stages.OfType<IDataFieldsWrite>().Where(w => ((Stage)w).Page == p).SelectMany(w => w.FieldsWrite).Distinct();
                var dataRead = data.Asset.Stages.OfType<IDataFieldsRead>().Where(w => ((Stage)w).Page == p).SelectMany(w => w.FieldsRead).Distinct();
                var readCollection = dataRead.Select(px => px.Split('.').First()).Distinct();

                return dataWritten.Except(dataRead).Except(readCollection).Where(w =>
                {
                    var df = p.FindDataField(w, true);
                    return df is not null && df.DataType != DataType.Runtime;
                }).Select(s => data.BuildFinding((Stage)p.FindDataStage(s, true)!, s));
            });
        }
    }
}
