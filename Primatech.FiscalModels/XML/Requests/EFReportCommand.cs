using System.Xml.Serialization;
using Primatech.FiscalModels.XML.Base;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "ReportCommand")]
    public class EFReportCommand : BaseEFCommand
    {
        [XmlElement(ElementName = "Report")]
        public EFReportRow Report { get; set; }
    }

    [XmlRoot(ElementName = "ReportRow")]
    public class EFReportRow
    {
        [XmlElement(ElementName = "CommandType")]
        public string CommandType { get; set; }
        [XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }
        [XmlElement(ElementName = "DateFrom")]
        public string DateFrom { get; set; }
        [XmlElement(ElementName = "DateTo")]
        public string DateTo { get; set; }
    }
}
