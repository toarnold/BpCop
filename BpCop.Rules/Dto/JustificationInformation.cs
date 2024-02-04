namespace BpCop.Rules.Dto
{
    public record JustificationInformation
    {
        public string Level { get; init; } = string.Empty;
        public string Rule { get; init; } = string.Empty;
        public string Asset { get; init; } = string.Empty;
        public string AssetType { get; init; } = string.Empty;
        public string Page { get; init; } = string.Empty;
        public string Stage { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public bool Used { get; init; }
    }
}
