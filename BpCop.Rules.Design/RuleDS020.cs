using BpCop.Common;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.SubStructures;
using BpCop.Common.BpModel.Types;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace BpCop.Rules.Design
{
    [Export(typeof(ICheckRule))]
    [ExportMetadata("RuleSet", "Design")]
    [ExportMetadata("RuleName", "DS020")]
    [ExportMetadata("Message", "Data/Collection isn't placed inside a block.")]
    [ExportMetadata("Description", "Place all data or collection items inside a block.")]
    [ExportMetadata("JustificationLevels", JustificationLevel.Global | JustificationLevel.Page | JustificationLevel.Stage)]
    [ExportMetadata("AppliesTo", AssetType.Process | AssetType.VBO)]
    [ExportMetadata("DefaultParameter", "")]
    internal sealed class RuleDS020 : ICheckRule
    {
        private static bool IsInside(Display outerBlock, Display innerDataField)
        {
            // Data field middle is (X/Y)
            // Block (X/Y) is the upper left corner
            return outerBlock.X <= innerDataField.X - innerDataField.W / 2
                && outerBlock.Y <= innerDataField.Y - innerDataField.H / 2
                && (outerBlock.X + outerBlock.W) >= (innerDataField.X + innerDataField.W / 2)
                && (outerBlock.Y + outerBlock.H) >= (innerDataField.Y + innerDataField.H / 2);
        }

        public IEnumerable<CheckResult> CheckRule(CheckRuleData data)
        {
            return data.Asset.Pages.Values.SelectMany(p =>
            {
                var datafields = data.Asset.Stages.OfType<IDataFields>().Where(w => ((Stage)w).Page == p);
                var blocks = data.Asset.Stages
                        .OfType<GenericStage>()
                        .Where(w => w.Page == p && w.StageType == "Block")
                        .Cast<Stage>()
                        .ToList();
                return datafields.Where(df => blocks.All(b => !IsInside(b.Display, ((Stage)df).Display))).Select(s => data.BuildFinding((Stage)s));
            });
        }
    }
}
