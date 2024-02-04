using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Console.Common.Output
{
    internal sealed class OutputHtml: OutputBase, IOutput
    {
        public void Append<T>(string caption, IList<T> values)
        {
            var doc = new XDocument();
            doc.Add(new XElement("table", new XAttribute("class", "table" + caption),
                new XElement("tr", new XAttribute("class", "tableRow"), GetColumns<T>().Select(c => new XElement("th", new XAttribute("class", "tableHeadline"), c))),
                values.Select(row => new XElement("tr", new XAttribute("class", "tableRow"), Values(row).Select(v => new XElement("td", new XAttribute("class", "tableCell"), v))))
                ));
            _output.Add(doc.ToString());
        }
    }
}
