using System;

namespace BpCop.Common
{
    [Flags]
    public enum JustificationLevel
    {
        None = 0,
        Global = 1,
        Page = 2,
        Stage = 4,
        AppModel = 8
    }
}
