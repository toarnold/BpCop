using BpCop.Common.BpModel.SubStructures;
using System.Collections.Generic;

namespace BpCop.Common.BpModel.Interfaces
{
    public interface IExpressions
    {
        IEnumerable<Expression> Expressions { get; }
    }
}
