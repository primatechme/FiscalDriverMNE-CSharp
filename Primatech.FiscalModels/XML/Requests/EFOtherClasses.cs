using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "Message")]
    public class EFMessage
    {
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }

    [XmlRoot(ElementName = "Footer")]
    public class EFFooter
    {
        [XmlElement(ElementName = "Message")]
        public List<string> Message { get; set; }
    }


    [XmlRoot(ElementName = "Currency")]
    public class EFCurrency
    {
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "Rate")]
        public decimal Rate { get; set; }
    }
}
