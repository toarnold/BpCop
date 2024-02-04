using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using BpCop.Common.BpModel.SubStructures;

namespace BpCop.Common.BpModel.Stages
{
    public abstract class Stage
    {
        /// <summary>
        /// Display name in BluePrism
        /// </summary>
        public string StageName { get; init; }

        /// <summary>
        /// Type name of the stage
        /// </summary>
        public string StageType { get; init; }

        /// <summary>
        /// The content of the stage  "Description" property
        /// </summary>
        public string Narrative { get; init; }

        /// <summary>
        /// Unique id of this stage
        /// </summary>
        public Guid StageId { get; init; }

        /// <summary>
        /// Id of the corresponding page
        /// </summary>
        public Guid SubSheetId { get; init; }

        /// <summary>
        /// Reference to the "Next" stage (true-case for decisions; "choice end" for choices)
        /// </summary>
        virtual public Guid OnSuccess { get; init; }

        /// <summary>
        /// Size and Position in BluePrism designer
        /// </summary>
        public Display Display { get; init; }

        /// <summary>
        /// Reference to the corresponding page
        /// </summary>
        public Page Page => Process.Pages[SubSheetId];

        /// <summary>
        /// Reference to the "root" process node
        /// </summary>
        public BpProcess Process { get; init; }

        /// <summary>
        /// Returns all possible callees
        /// </summary>
        public virtual IEnumerable<Guid> NextStages
        {
            get
            {
                var next = Page.Process.GetStage(OnSuccess);
                return (next is null ? Guid.Empty : next.StageId).SingleEnumerable();
            }
        }

        /// <summary>
        /// Returns all possible callers
        /// </summary>
        public IEnumerable<Guid> PreviousStages => Page.Stages.Values.Where(w => w.NextStages.Any(a => a == StageId)).Select(s => s.StageId);

        /// <summary>
        /// Get the "Stage logging" stage property. "Disabled", "Enabled" or "Errors only"
        /// </summary>
        public string Logging
        {
            get
            {
                var loginihit = StageElement.Element("loginhibit");
                if (loginihit is null)
                {
                    return "Enabled";
                }
                return loginihit.GetTypedAttributeValue<bool>("onsuccess") ? "Errors only" : "Disabled";
            }
        }

        /// <summary>
        /// Raw xml data of this stage
        /// </summary>
        public XElement StageElement { get; init; }

        protected Stage(BpProcess process, XElement element)
        {
            if (process is null)
            {
                throw new ArgumentNullException(nameof(process));
            }
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            StageElement = element;
            Process = process;
            SubSheetId = element.GetTypedElementValue<Guid>("subsheetid");
            StageId = element.GetTypedAttributeValue<Guid>("stageid");
            StageName = element.GetStringAttributeValue("name");
            StageType = element.GetStringAttributeValue("type");
            Narrative = element.GetStringElementValue("narrative");
            OnSuccess = element.GetTypedElementValue<Guid>("onsuccess");
            var displayelement = element.Element("display");
            Display = new Display(displayelement ?? element, displayelement is null);
        }

        /// <summary>
        /// Invokes contructor of special type if exists, or generates generic stage type
        /// </summary>
        /// <param name="process">Process information object</param>
        /// <param name="element">Xml element to contruct from</param>
        /// <returns>stage instance</returns>
        internal static Stage LoadFrom(BpProcess process, XElement element)
        {
            var targetType = Type.GetType($"BpCop.Common.BpModel.Stages.{element.GetStringAttributeValue("type")}Stage") ?? typeof(GenericStage);

            // Invoke internal constructor
            return (Stage)Activator.CreateInstance(targetType, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { process, element }, CultureInfo.CurrentCulture);
        }
    }

}
