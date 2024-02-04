using BpCop.Common.BpModel.SubStructures;
using System.Collections.Generic;

namespace BpCop.Common.BpModel.Interfaces
{
    public interface IDataSources
    {
        IEnumerable<DataSource> DataSources { get; }
    }
}
