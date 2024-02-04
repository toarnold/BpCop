using System.Collections.Generic;
using System.Xml.Linq;
using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class ExceptionStage : Stage, IDataFieldsRead, IExpressions
    {
        private readonly XElement _exceptionElement;
        public string ExceptionType { get; init; }
        public Expression Expression { get; init; }
        public bool SaveScreenCapture { get; init; }
        public bool UseCurrent { get; init; }
        public IEnumerable<string> FieldsRead => Expression.UsedData;
        public IEnumerable<Expression> Expressions => Expression.SingleEnumerable();

        internal ExceptionStage(BpProcess process, XElement element) : base(process, element)
        {
            _exceptionElement = element.Element("exception");
            ExceptionType = _exceptionElement.GetStringAttributeValue("type");
            Expression = new Expression(_exceptionElement.GetStringAttributeValue("detail"));
            SaveScreenCapture = _exceptionElement.GetStringAttributeValue("savescreencapture") == "yes";
            UseCurrent = _exceptionElement.GetStringAttributeValue("usecurrent") == "yes";
        }
    }
}
