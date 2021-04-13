using Primatech.FiscalModels.XML.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Base
{
    [XmlRoot(ElementName = "EFCommand")]
    public class BaseEFCommand
    {
        [XmlElement(ElementName = "UID")]
        public string UID { get; set; }
        [XmlElement(ElementName = "PrinterId")]
        public string PrinterId { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
    }


    


    
}
