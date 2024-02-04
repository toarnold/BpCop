using BpCop.Common.BpModel.Interfaces;
using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.SubStructures;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace BpCop.Common.BpModel
{
    public static class Extensions
    {
        /// <summary>
        /// Gets child XElement value as typed value. Returns default(T) if XElement not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static T? GetTypedElementValue<T>(this XElement? element, string name)
        {
            var value = element?.Element(name)?.Value;
            return value is not null ? (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value) : default;
        }

        internal static string GetStringElementValue(this XElement element, string name) => GetTypedElementValue<string>(element, name) ?? string.Empty;

        /// <summary>
        /// Gets XAttribute value as typed value. Returns default(T) if XAttribute not exists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static T? GetTypedAttributeValue<T>(this XElement? element, string name)
        {
            var value = element?.Attribute(name)?.Value;
            return value is not null ? (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value) : default;
        }

        internal static string GetStringAttributeValue(this XElement element, string name) => GetTypedAttributeValue<string>(element, name) ?? string.Empty;

        public static IEnumerable<T> SingleEnumerable<T>(this T source) => Enumerable.Repeat(source, 1);

        public static IDataFields? FindDataStage(this Page page, string name, bool isprivate)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page));
            }
            return page.Process.Stages.OfType<IDataFields>().FirstOrDefault(w => w.IsPrivate == isprivate && (!isprivate || ((Stage)w).Page == page) && w.DataFields.Any(a => a.Name == name));
        }

        public static DataField? FindDataField(this Page page, string name, bool isprivate)
        {
            return page.FindDataStage(name, isprivate)?.DataFields.FirstOrDefault(f => f.Name == name);
        }

        public static DataField FindDataField(this Page page, string name)
        {
            return (page.FindDataField(name, true) ?? page.FindDataField(name, false)) ?? new DataField(Name: name, DataType: DataType.Runtime);
        }
    }
}
 