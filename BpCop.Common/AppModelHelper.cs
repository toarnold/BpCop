using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BpCop.Common
{
    public static class AppModelHelper
    {
        public static string BuildPath(this XElement node)
        {
            if (node is null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            return string.Join("/", node.AncestorsAndSelf().Where(e => e.Name.LocalName == "element" || e.Name.LocalName == "group").Reverse().Select(s => s.Attribute("name").Value));
        }

        public static string BuildPath(this XElement node, Guid id) => node.XPathSelectElement($"//element[id='{id}']").BuildPath();
    }
}
