using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "User")]
    public class EFUser
    {
        [XmlElement(ElementName = "UserCode")]
        public string UserCode { get; set; }
        [XmlElement(ElementName = "UserName")]
        public string UserName { get; set; }
    }
}
