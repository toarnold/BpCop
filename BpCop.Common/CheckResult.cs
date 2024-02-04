using System;

namespace BpCop.Common
{
    public class CheckResult
    {
        public string AssetName { get; set; } = string.Empty;
        public string AssetType { get; set; } = string.Empty;
        public string RuleName { get; set; } = string.Empty;
        public string PageName { get; set; } = string.Empty;
        public string StageName { get; set; } = string.Empty;
        public string FormattedMessage { get; set; } = string.Empty;
        public Guid SheetId { get; set; }
        public Guid StageId { get; set; }
    }
}
