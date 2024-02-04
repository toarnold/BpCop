using BpCop.Common.BpModel;

namespace BpCop.Common
{
    public record CheckRuleData(BpProcess Asset, ICheckRuleMetadata Metadata, dynamic Parameter);
}
