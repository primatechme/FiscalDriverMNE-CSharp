using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "DepositCommand")]
    public class EFDepositCommand
    {
        [XmlElement(ElementName = "UID")]
        public string Uid { get; set; }
        [XmlElement(ElementName = "ENUIdentifier")]
        public string ENUIdentifier { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "TestMode")]
        public bool TestMode { get; set; }
        [XmlElement(ElementName = "DepositType")]
        public string DepositType { get; set; }


        [XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }
        public DateTime DateSend { get; set; }
        [XmlElement(ElementName = "User")]
        public EFUser User { get; set; }
    }
}
