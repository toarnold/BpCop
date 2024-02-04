using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class MultipleCalculationStage : Stage, IDataFieldsRead, IDataFieldsWrite, ICalculation, IExpressions
    {
        public IList<Calculation> Calculations { get; init; }
        public IEnumerable<string> FieldsRead => Calculations.SelectMany(c => c.Expression.UsedData).Distinct();
        public IEnumerable<string> FieldsWrite => Calculations.Select(s => s.Stage).Where(w => !string.IsNullOrWhiteSpace(w)).Distinct();
        public IEnumerable<Expression> Expressions => Calculations.Select(c => c.Expression);

        internal MultipleCalculationStage(BpProcess process, XElement element) : base(process, element)
        {
            Calculations = element.Element("steps").Elements("calculation").Select(s => new Calculation(s)).ToList();
        }
    }
}
