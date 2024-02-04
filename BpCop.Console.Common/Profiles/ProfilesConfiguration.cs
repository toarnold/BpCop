using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace BpCop.Console.Common.Profiles
{
    public class ProfilesConfiguration : IConfigurationSectionHandler
    {
        private static IEnumerable<XmlElement> SaveSelectNode(string xpath, XmlNode section) => section.SelectNodes(xpath)?.OfType<XmlElement>() ?? Enumerable.Empty<XmlElement>();

        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section is null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            return SaveSelectNode("Profile", section)
                .ToDictionary(k => k.GetAttribute("name"),
                        v => SaveSelectNode("add", v).ToDictionary(k => $"{(k.GetAttribute("key").Length == 1 ? "-" : "--")}{k.GetAttribute("key")}", v2 => v2.GetAttribute("value")));
        }
    }
}
