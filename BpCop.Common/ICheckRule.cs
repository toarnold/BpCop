using System.Collections.Generic;

namespace BpCop.Common
{
    public interface ICheckRule
    {
        IEnumerable<CheckResult> CheckRule(CheckRuleData data);
    }
}
