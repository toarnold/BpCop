using System.Globalization;
using System.Xml.Linq;

namespace BpCop.Common.BpModel.Stages
{
    public sealed class RecoverStage : Stage
    {
        /// <summary>
        /// Represents: Limit the number of retries this recover is attempted
        /// </summary>
        public bool LimitRetries { get; init; }

        /// <summary>
        /// Represents: Maximum Attempts
        /// </summary>
        public int Attemps { get; init; }

        /// <summary>
        /// New in BP 7.2
        /// Represents: Raise exception to parent page when maximum attempts reached
        /// </summary>
        public bool BubbleException { get; init; }

        internal RecoverStage(BpProcess process, XElement element) : base(process, element)
        {
            LimitRetries = element.Element("attempts") is not null;
            Attemps = LimitRetries ? int.Parse(element.Element("attempts")?.Value ?? "0", CultureInfo.InvariantCulture) : 0;
            BubbleException = LimitRetries && element.Element("bubbleException")?.Value == "True";
        }
    }

    /*
     <stage stageid="4d69232e-e926-4813-a674-912aa95e049d" name="Recover" type="Recover">
        <subsheetid>173215ee-e4ce-438c-b3b0-8d5199d13818</subsheetid>
        <loginhibit />
        <display x="45" y="-75" />
        <attempts>1</attempts>
        <bubbleException>True</bubbleException>
    </stage> 
     */
}
