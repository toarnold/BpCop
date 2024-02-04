using BpCop.DataProviders.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BpCop.DataProviders
{
    public static class FileProvider
    {
        static readonly XNamespace ReleaseNs = "http://www.blueprism.co.uk/product/release";
        static readonly XNamespace AssetNs = "http://www.blueprism.co.uk/product/process";

        private static XElement RemoveAllNamespaces(XElement element)
        {
            foreach (var node in element.DescendantsAndSelf())
            {
                node.Name = node.Name.LocalName;
            }
            return element;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "Array only used once.")]
        private static XDocument? GetDocument(string path, TextWriter twerror)
        {
            if (!File.Exists(path))
            {
                twerror.WriteLine($"File '{path}' doesn't exist.");
                return null;
            }

            var extension = Path.GetExtension(path);
            if (!new[] { ".xml", ".bpobject", ".bpprocess", ".bprelease" }.Any(x => x.Equals(extension, StringComparison.OrdinalIgnoreCase)))
            {
                twerror.WriteLine($"Unsupported file extension {extension}.");
                return null;
            }

            var xml = File.ReadAllText(path);
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Parse(xml);
            }
            catch (XmlException xEx)
            {
                twerror.WriteLine($"'{path}' isn't a valid xml file. (Message: {xEx.Message})");
                return null;
            }
            return xdoc;
        }

        public static IEnumerable<AssetInformation> GetAssetsInformation(string path, TextWriter twerror)
        {
            if (twerror is null)
            {
                throw new ArgumentNullException(nameof(twerror));
            }

            var xdoc = GetDocument(path, twerror);
            if (xdoc is null)
            {
                return Enumerable.Empty<AssetInformation>();
            }

            var extension = Path.GetExtension(path);
            if (extension.Equals(".bprelease", StringComparison.OrdinalIgnoreCase))
            {
                // get all assets from release file
                var content = xdoc.Root?.Element(ReleaseNs + "contents");
                if (content is null)
                {
                    return Enumerable.Empty<AssetInformation>();
                }

                return content.Elements(AssetNs + "object")
                    .Select(s => new AssetInformation("O", s.Attribute("name")?.Value ?? string.Empty, Guid.Parse(s.Attribute("id")?.Value ?? Guid.Empty.ToString())))
                    .Concat(content.Elements(AssetNs + "process")
                        .Select(s => new AssetInformation("P", s.Attribute("name")?.Value ?? string.Empty, Guid.Parse(s.Attribute("id")?.Value ?? Guid.Empty.ToString())))
                    );
            }
            else
            {
                return new[] {
                    new AssetInformation(xdoc.Root.Attribute("type")?.Value == "object"?"O":"P", xdoc.Root.Attribute("name")?.Value ?? string.Empty, Guid.Parse(xdoc.Root.Attribute("preferredid")?.Value ?? Guid.Empty.ToString()))
                };
            }
        }

        public static IEnumerable<ProcessInformation> GetProcesses(string path, IEnumerable<string> names, TextWriter twerror)
        {
            if (twerror is null)
            {
                throw new ArgumentNullException(nameof(twerror));
            }

            var xdoc = GetDocument(path, twerror);
            if (xdoc is null)
            {
                return Enumerable.Empty<ProcessInformation>();
            }

            var extension = Path.GetExtension(path);
            if (extension.Equals(".bprelease", StringComparison.OrdinalIgnoreCase))
            {
                var content = xdoc.Root?.Element(ReleaseNs + "contents");
                if (content is null)
                {
                    return Enumerable.Empty<ProcessInformation>();
                }
                return content.Elements(AssetNs + "object")
                    .Concat(content.Elements(AssetNs + "process"))
                    .Where(w => !names.Any() || names.Contains(w.Attribute("name")?.Value))
                    .Select(s => new ProcessInformation(
                        Guid.Parse(s.Attribute("id")?.Value ?? Guid.Empty.ToString()),
                        s.Attribute("name")?.Value ?? string.Empty,
                        RemoveAllNamespaces(s.Elements().First()).ToString(),
                        s.Name.LocalName == "process" ? "P" : "O",
                        Convert.ToBoolean(s.Attribute("published")?.Value ?? false.ToString(), CultureInfo.InvariantCulture)));
            }
            else // assume valid object/process file
            {
                return new[] {
                    new ProcessInformation(Guid.Parse(xdoc.Root.Attribute("preferredid")?.Value ?? Guid.Empty.ToString()),xdoc.Root.Attribute("name")?.Value ?? string.Empty,xdoc!.ToString(),xdoc.Root.Attribute("content")?.Value == "object"?"O":"P",xdoc.Root.Attribute("published_type")?.Value=="2")
                    };
            }
        }
    }
}
