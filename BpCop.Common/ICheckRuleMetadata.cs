using BpCop.Common.BpModel.Types;

namespace BpCop.Common
{
    public interface ICheckRuleMetadata
    {
        string RuleName { get; }
        string RuleSet { get; }
        string Message { get; }
        string Description { get; }
        JustificationLevel JustificationLevels { get; }
        AssetType AppliesTo { get; }
        string DefaultParameter { get; }
    }
}
