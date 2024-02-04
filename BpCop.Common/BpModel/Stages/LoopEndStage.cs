using System;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class LoopEndStage : Stage
    {
        public Guid GroupId { get; init; }

        internal LoopEndStage(BpProcess process, XElement element) : base(process, element)
        {
            GroupId = element.GetTypedElementValue<Guid>("groupid");
        }
    }
}
