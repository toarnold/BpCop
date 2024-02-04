using System.Collections.Generic;

namespace BpCop.Common.BpModel.Interfaces
{
    public interface IDataFieldsWrite
    {
        IEnumerable<string> FieldsWrite { get; }
    }
}
