using System.Collections.Generic;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class DataStage : Stage, IDataFields
    {
        public string DataType { get; init; }
        public string Exposure { get; init; }
        public bool IsPrivate { get; init; }

        public IEnumerable<DataField> DataFields => new DataField(StageName, DataType).SingleEnumerable();

        internal DataStage(BpProcess process, XElement element) : base(process, element)
        {
            DataType = element.GetStringElementValue("datatype");
            Exposure = element.GetStringElementValue("exposure");
            IsPrivate = element.Element("private") is not null;
        }
    }
}
