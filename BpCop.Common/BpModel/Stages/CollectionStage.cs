using System.Collections.Generic;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;
using BpCop.Common.BpModel.Types;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class CollectionStage : Stage, IDataFields
    {
        public bool IsPrivate { get; init; }
        public bool IsTyped { get; init; }
        public IEnumerable<DataField> DataFields { get; init; }

        private void AppendFields(XElement fieldsContainer, string prefix)
        {
            foreach (var element in fieldsContainer.Elements("field"))
            {
                var field = new DataField($"{prefix}.{element.GetStringAttributeValue("name")}", element.GetStringAttributeValue("type"));
                ((List<DataField>)DataFields).Add(field);
                if (field.DataType == DataType.Collection)
                {
                    AppendFields(element, field.Name);
                }
            }
        }

        internal CollectionStage(BpProcess process, XElement element) : base(process, element)
        {
            IsPrivate = element.Element("private") is not null;
            IsTyped = element.Element("collectioninfo") is not null;
            DataFields = new List<DataField> { new(StageName, DataType.Collection) };
            var fields = element.Element("collectioninfo");
            if (fields is not null)
            {
                AppendFields(fields, StageName);
            }

            /*
    <collectioninfo>
      <field name="Key" type="text" />
      <field name="Group" type="collection">
        <field name="Nr" type="number" />
        <field name="Text" type="text" />
      </field>
    </collectioninfo>
             */
        }
    }
}
