using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BpCop.Common.BpModel.Stages;

namespace BpCop.Common.BpModel
{
    public class Page
    {
        /// <summary>
        /// Display name in BuePrism
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Id of this page
        /// </summary>
        public Guid SubSheetId { get; init; }

        /// <summary>
        /// "Main" for Main/Initialise page
        /// "CleanUp" for objects clean up page
        /// "Normal" else
        /// </summary>
        public string PageType { get; init; }

        /// <summary>
        /// True if process page (even "Main") or object published page
        /// </summary>
        public bool IsPublished => _isPublished ?? IsMainPage;
        private readonly bool? _isPublished;

        /// <summary>
        /// True if page is a process "Main" or object "initialise"
        /// </summary>
        public bool IsMainPage => SubSheetId == Guid.Empty;

        /// <summary>
        /// Enumerates all stages of the page
        /// </summary>
        public IDictionary<Guid, Stage> Stages { get; init; }

        private readonly XElement _element;

        /// <summary>
        /// Reference to the "root" process node
        /// </summary>
        public BpProcess Process { get; init; }

        internal Page(BpProcess process, XElement? element, bool isProcess = false)
        {
            _element = element ?? new XElement("subsheet",
                    new XElement("name", isProcess ? "Main Page" : "Initialise"),
                    new XAttribute("type", "Main"));
            Name = _element.GetStringElementValue("name");
            SubSheetId = _element.GetTypedAttributeValue<Guid>("subsheetid");
            PageType = _element.GetStringAttributeValue("type");
            _isPublished = _element.GetTypedAttributeValue<bool?>("published");
            Process = process;
            Stages = process.Stages.Where(k => k.SubSheetId == SubSheetId).ToDictionary(k => k.StageId, v => v);
        }

    }
}
