using System;
using System.Collections.Generic;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class LoopStartStage : Stage, IDataFieldsRead
    {
        public Guid GroupId { get; init; }
        public string Stage { get; init; }

        public IEnumerable<string> FieldsRead => Stage.SingleEnumerable();

        internal LoopStartStage(BpProcess process, XElement element) : base(process, element)
        {
            GroupId = StageElement.GetTypedElementValue<Guid>("groupid");
            Stage = StageElement.GetStringElementValue("loopdata");
        }
    }
}
