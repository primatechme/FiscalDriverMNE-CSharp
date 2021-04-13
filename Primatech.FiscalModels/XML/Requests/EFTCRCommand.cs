using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "TCRCommand")]
    public class EFTCRCommand
    {
        [XmlElement(ElementName = "UID")]
        public string Uid { get; set; }
        [XmlElement(ElementName = "BusinessUnitCode")]
        public string BusinessUnitCode { get; set; }

        [XmlElement(ElementName = "ENUIdentifier")]
        public string ENUIdentifier { get; set; }
        [XmlElement(ElementName = "ExistingTCRCode")]
        public string ExistingTCRCode { get; set; }

        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "TestMode")]
        public bool TestMode { get; set; }
        [XmlElement(ElementName = "DateSend")]
        public DateTime DateSend { get; set; }
        [XmlElement(ElementName = "VATNumber")]
        public string VATNumber { get; set; }
        [XmlElement(ElementName = "InternalIdentifier")]
        public string InternalIdentifier { get; set; }
        [XmlElement(ElementName = "ValidFrom")]
        public DateTime ValidFrom { get; set; }
        [XmlElement(ElementName = "ValidTo")]
        public DateTime? ValidTo { get; set; }
        [XmlIgnore]
        public bool ValidToSpecified
        {
            get
            {
                return (ValidTo != null && ValidTo.HasValue);
            }
        }


    }
}
