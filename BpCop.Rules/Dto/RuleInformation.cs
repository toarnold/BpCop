namespace BpCop.Rules.Dto
{
    public record RuleInformation
    {
        public string Set { get; init; } = string.Empty;
        public string Rule { get; init; } = string.Empty;
        public string Target { get; init; } = string.Empty;
        public string JustificationLevels { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Parameter { get; init; } = string.Empty;
    }
}
