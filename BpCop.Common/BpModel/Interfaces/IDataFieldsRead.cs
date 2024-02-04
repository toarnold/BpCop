using System.Collections.Generic;

namespace BpCop.Common.BpModel.Interfaces
{
    public interface IDataFieldsRead
    {
        IEnumerable<string> FieldsRead { get; }
    }
}
