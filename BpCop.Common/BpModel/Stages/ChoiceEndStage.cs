using System;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class ChoiceEndStage : Stage
    {
        public Guid GroupId { get; init; }

        internal ChoiceEndStage(BpProcess process, XElement element) : base(process, element)
        {
            GroupId = element.GetTypedElementValue<Guid>("groupid");
        }

        /*
<stage stageid="e4f6482c-6875-4418-82ef-1c227c7bb5dc" name="Otherwise1" type="ChoiceEnd">
  <subsheetid>aa6b3806-af47-433c-99a0-d20cf36abe26</subsheetid>
  <display x="165" y="120" />
  <onsuccess>3421157f-067d-4c3e-99c4-b58bd5bb0346</onsuccess>
  <groupid>13edf4e3-a43d-46f7-af9c-ffb3a6e6c051</groupid>
</stage>
        */
    }
}
