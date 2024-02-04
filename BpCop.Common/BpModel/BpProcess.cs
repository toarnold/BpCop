using BpCop.Common.BpModel.Stages;
using BpCop.Common.BpModel.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BpCop.Common.BpModel
{
    public class BpProcess
    {
        /// <summary>
        /// Name of the process or object
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Description of the Initialise/main page
        /// </summary>
        public string Narrative { get; init; }

        /// <summary>
        /// Asset type (object or process)
        /// </summary>
        public AssetType AssetType => _assetType == "object" ? AssetType.VBO : AssetType.Process;
        private readonly string _assetType;

        /// <summary>
        /// Runmode in case of an object, null for a process
        /// </summary>
        public string Runmode { get; init; }

        private Guid? _externalId;
        /// <summary>
        /// Id of the process/object as a proposal
        /// </summary>
        public Guid PreferredId => _preferredId;
        private readonly Guid _preferredId;

        /// <summary>
        /// True if asset is object and an AppModel is defined
        /// </summary>
        public bool HasAppModel => _hasAppModel;
        private readonly bool _hasAppModel;

        /// <summary>
        /// Raw parsed xml to an AppModel, if exists, null else
        /// </summary>
        public XElement AppModel { get; init; }

        /// <summary>
        /// Dictionary PageId -> Page, Id is Guid.Empty for Main/Initialise page.
        /// </summary>
        public IDictionary<Guid, Page> Pages { get; init; }

        private readonly IDictionary<Guid, Stage> _stages;
        /// <summary>
        /// Get a Stage by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Stage? GetStage(Guid id) => id != Guid.Empty ? _stages[id] : null;

        /// <summary>
        /// Collection of all stages in this asset
        /// </summary>
        public IEnumerable<Stage> Stages => _stages.Values;

        /// <summary>
        /// Collections of all set justification in this asset
        /// </summary>
        public IDictionary<string, IOrderedEnumerable<Justification>> Justifications { get; init; }

        /// <summary>
        /// True if asset is process and published, False else
        /// </summary>
        public bool IsPublished { get; init; }

        private readonly XElement _element;
        private readonly XDocument _document;

        public BpProcess(string processxml, Guid? processId, bool isPublished)
        {
            IsPublished = isPublished;
            _externalId = processId;
            _document = XDocument.Parse(processxml);
            _element = _document.Root;
            Name = _element.GetStringAttributeValue("name");
            Narrative = _element.GetStringAttributeValue("narrative");
            _assetType = _element.GetStringAttributeValue("type");
            Runmode = _element.GetStringAttributeValue("runmode");
            _preferredId = _element.Attribute("preferredid") is not null ? _element.GetTypedAttributeValue<Guid>("preferredid") : _externalId ?? Guid.Empty;
            _hasAppModel = AssetType == AssetType.VBO && _element.Element("appdef")?.Element("apptypeinfo") is not null;

            AppModel = _element.Element("appdef");

            _stages = _element.Elements("stage")
                .Select(s => Stage.LoadFrom(this, s))
                .ToDictionary(k => k.StageId, v => v);

            Pages = new Page(this, null, AssetType == AssetType.Process).SingleEnumerable()
                .Concat(_element.Elements("subsheet")
                .Select(s => new Page(this, s)))
                .ToDictionary(k => k.SubSheetId, v => v);

            var justifications = Stages
                .Where(s => s.StageType == "Note")
                .Cast<GenericStage>()
                .SelectMany(JustificationFactory.BuildFromNote);

            if (HasAppModel)
            {
                justifications = justifications.Concat(AppModel.XPathSelectElements("//element[description/text()]").SelectMany(s => JustificationFactory.BuildFromAppmodelElement(this, s)));
            }

            Justifications = justifications.GroupBy(g => g.RuleName).ToDictionary(k => k.Key, v => v.OrderBy(k => k.Level));
        }
    }
}
