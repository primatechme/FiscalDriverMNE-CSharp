using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "Client")]
    public class EFClient
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "IDType")]
        public string IDType { get; set; }
        [XmlElement(ElementName = "IDValue")]
        public string IDValue { get; set; }
        [XmlElement(ElementName = "Country")]
        public string Country { get; set; }
        [XmlElement(ElementName = "Town")]
        public string Town { get; set; }
        [XmlElement(ElementName = "Address")]
        public string Address { get; set; }
    }
}
