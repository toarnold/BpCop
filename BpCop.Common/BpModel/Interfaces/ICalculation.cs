using System.Collections.Generic;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Interfaces
{
    public interface ICalculation
    {
        IList<Calculation> Calculations { get; }
    }
}
