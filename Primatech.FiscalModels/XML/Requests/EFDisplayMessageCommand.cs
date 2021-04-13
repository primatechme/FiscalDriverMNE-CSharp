using Primatech.FiscalModels.XML.Base;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "DisplayMessageCommand")]
    public class EFDisplayMessageCommand : BaseEFCommand
    {
        [XmlElement(ElementName = "Row1")]
        public string Row1 { get; set; }
        [XmlElement(ElementName = "Row2")]
        public string Row2 { get; set; }
    }

}
