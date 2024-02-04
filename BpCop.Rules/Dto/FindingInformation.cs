namespace BpCop.Rules.Dto
{
    public record FindingInformation
    {
        public string Rule { get; init; } = string.Empty;
        public string Asset { get; init; } = string.Empty;
        public string AssetType { get; init; } = string.Empty;
        public string Page { get; init; } = string.Empty;
        public string Stage { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string JustificationLevel { get; init; } = string.Empty;
        public string Justification { get; init; } = string.Empty;
    }
}
