using System;
using System.Collections.Generic;
using System.Linq;

namespace BpCop.Console.Common.Output
{
    public interface IOutput
    {
        void Append<T>(string caption, IList<T> values);
    }

    abstract internal class OutputBase
    {
        protected IList<string> _output = new List<string>();

        protected static readonly HashSet<Type> NumericTypes =
        [
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        ];

        protected static IEnumerable<string> GetColumns<T>() => typeof(T).GetProperties().Select(x => x.Name);

        protected static object GetColumnValue<T>(T target, string column) => typeof(T).GetProperty(column).GetValue(target, null);

        protected static object[] Values<T>(T target) => GetColumns<T>().Select(r => GetColumnValue(target, r)).ToArray();

        protected static bool IsNumeric(Type t) => NumericTypes.Contains(t);

        public override string ToString() => string.Join(Environment.NewLine, _output);
    }

    abstract public class OutputFactory
    {
        public static IOutput Create(Format format)
        {
            return format switch
            {
                Format.Default => new OutputDefault(),
                Format.Html => new OutputHtml(),
                Format.Json => new OutputJson(),
                Format.Markdown => new OutputMarkdown(),
                Format.Csv => new OutputCsv(),
                Format.Minimal => new OutputMinimal(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
