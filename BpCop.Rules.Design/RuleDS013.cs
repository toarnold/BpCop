using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS013")]
    [ExportMetadata("Message", "Avoid exception type '{0}'")]
    [ExportMetadata("Description", "Avoid countless exception types.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "{\"AllowedExceptions\": [\"System Exception\",\"Business Exception\"]}")]
    internal sealed class RuleDS013 : ICheckRule
    {
        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages.OfType<ExceptionStage>()
                .Where(w => !w.UseCurrent && !data.Parameter.AllowedExceptions.ToObject<IList<string>>().Contains(w.ExceptionType))
                .Select(s => data.BuildFinding(s, s.ExceptionType));
        }
    }
}
