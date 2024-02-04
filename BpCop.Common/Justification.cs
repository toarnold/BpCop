namespace BpCop.Common
{
    public class Justification
    {
        public string AssetName { get; init; } = string.Empty;
        public string AssetType { get; init; } = string.Empty;
        public string PageName { get; init; } = string.Empty;
        public string StageName { get; init; } = string.Empty;
        public string RuleName { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public JustificationLevel Level { get; init; }
        public int UsageCount { get; set; }
    }
}
