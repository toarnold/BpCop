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
    [ExportMetadata("RuleName", "DS019")]
    [ExportMetadata("Message", "Global data '{0}' is written only")]
    [ExportMetadata("Description", "A global data field or collection will be written, but never read.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS019 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            var dataWritten = data.Asset.Stages.OfType<IDataFieldsWrite>().SelectMany(w => w.FieldsWrite).Distinct();
            var dataRead = data.Asset.Stages.OfType<IDataFieldsRead>().SelectMany(w => w.FieldsRead).Distinct();
            var readCollection = dataRead.Select(px => px.Split('.').First()).Distinct();

            return dataWritten.Except(dataRead).Except(readCollection).Where(w =>
            {
                var df = data.Asset.Pages.First().Value.FindDataField(w, false);
                return df is not null && df.DataType != DataType.Runtime;
            }).Select(s => data.BuildFinding((Stage)data.Asset.Pages.First().Value.FindDataStage(s, false)!, s));
        }
    }
}
