using System.Collections.Generic;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Interfaces
{
    /// <summary>
    /// Describes all defined data fields from a collection or data stage
    /// </summary>
    public interface IDataFields
    {
        IEnumerable<DataField> DataFields { get; }
        bool IsPrivate { get; }
    }
}
