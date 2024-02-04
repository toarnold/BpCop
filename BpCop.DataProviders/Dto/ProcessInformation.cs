using System;

namespace BpCop.DataProviders.Dto
{
    public record ProcessInformation(Guid ProcessId, string Name, string ProcessXml, string ProcessType, bool IsPublished);
}
