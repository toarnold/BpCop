using BpCop.Common;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;

namespace BpCop.Rules.Programming
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Programming")]
    [ExportMetadata("RuleName", "PG014")]
    [ExportMetadata("Message", "Default name detected (old style)")]
    [ExportMetadata("Description", "Action name seems to be generated. Use a descriptive name instead")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RulePG014 : ICheckRule
    {
        private readonly Regex NumericExpression = new(@"^\w+\d+$", RegexOptions.Compiled);

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Stages
                .Where(stage => stage is ReadStage || 
                        stage is WriteStage || 
                        stage is NavigateStage || 
                        stage is DecisionStage || 
                        stage is ChoiceStartStage || 
                        stage is ChoiceEndStage || 
                        stage is CalculationStage ||
                        stage is MultipleCalculationStage)
                .Where(w => NumericExpression.IsMatch(w.StageName))
                .Select(s => data.BuildFinding(s));
        }
    }
}
