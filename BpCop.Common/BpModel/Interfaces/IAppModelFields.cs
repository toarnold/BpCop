using System;
using System.Collections.Generic;

namespace BpCop.Common.BpModel.Interfaces
{
    public interface IAppModelFields
    {
        IEnumerable<Guid> AppModelFields { get; }
    }
}
