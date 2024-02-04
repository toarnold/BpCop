using BpCop.Common.BpModel;
using BpCop.Common.BpModel.Stages;
using System;
using System.Globalization;

namespace BpCop.Common
{
    public static class CheckRuleDataExtensions
    {
        private static CheckResult BuildFinding(this CheckRuleData data, Page? page, Stage? stage, params object[] parameter)
        {
            return new CheckResult
            {
                AssetName = data.Asset.Name,
                AssetType = data.Asset.AssetType.ToString(),
                RuleName = data.Metadata.RuleName,
                SheetId = page?.SubSheetId ?? Guid.Empty,
                StageId = stage?.StageId ?? Guid.Empty,
                FormattedMessage = string.Format(CultureInfo.InvariantCulture, data.Metadata.Message, parameter),
                PageName = page?.Name ?? string.Empty,
                StageName = stage?.StageName ?? string.Empty
            };
        }

        public static CheckResult BuildFinding(this CheckRuleData data, Page page, params object[] parameter)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return data.BuildFinding(page, null, parameter);
        }

        public static CheckResult BuildFinding(this CheckRuleData data, Stage stage, params object[] parameter)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (stage is null)
            {
                throw new ArgumentNullException(nameof(stage));
            }
            return data.BuildFinding(stage.Page, stage, parameter);
        }

        public static CheckResult BuildFinding(this CheckRuleData data, params object[] parameter)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return data.BuildFinding(null, null, parameter);
        }

        public static CheckResult BuildFindingFromAppModel(this CheckRuleData data, string path, params object[] parameter)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var res = data.BuildFinding(null, null, parameter);
            res.StageName = path;
            return res;
        }

    }
}
