using System.Collections.Generic;

namespace BpCop.Rules.Dto
{
    public record AnalysisInformation(IList<FindingInformation> Findings, IList<JustificationInformation> Justifications, IList<RuleInformation> Rules);
}
