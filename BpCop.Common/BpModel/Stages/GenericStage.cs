using System.Xml.Linq;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class GenericStage : Stage
    {
        internal GenericStage(BpProcess process, XElement element) : base(process, element)
        {
        }
    }
}
