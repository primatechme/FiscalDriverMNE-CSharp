using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlInclude(typeof(EFiscalReceiptCommand))]
    [Serializable]
    [XmlRoot(ElementName = "FiscalReceiptCommand")]
    public class EFiscalReceiptCommand 
    {
        [XmlIgnore]
        [JsonIgnore]
        public string Id { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public string ServiceUser { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        public DateTime RequestDate { get; set; }
        [XmlElement(ElementName = "UID")]
        public string Uid { get; set; }
        [XmlElement(ElementName = "IKOF")]
        public string IKOF { get; set; }
        [XmlElement(ElementName = "ENUIdentifier")]
        public string ENUIdentifier { get; set; }
        [XmlElement(ElementName = "DocumentType")]
        public string DocumentType { get; set; }
        [XmlElement(ElementName = "DocumentNumber")]
        public int DocumentNumber { get; set; }
        [XmlElement(ElementName = "BasePriceIsWithoutTax")]
        public bool BasePriceIsWithoutTax { get; set; }
        [XmlElement(ElementName = "IsNoCashReceipt")]
        public bool IsNoCashReceipt { get; set; }
        [XmlElement(ElementName = "DateSend")]
        public DateTime DateSend { get; set; }
        [XmlElement(ElementName = "ConnectedDocuments")]
        public EFConnectedDocuments ConnectedDocuments { get; set; }
        [XmlElement(ElementName = "IsReverseCharge")]
        public bool? IsReverseCharge { get; set; }
        [XmlIgnore]
        public bool IsReverseChargeSpecified
        {
            get
            {
                return (IsReverseCharge != null && IsReverseCharge.HasValue);
            }
        }
        [XmlElement(ElementName = "DueDate")]
        public DateTime? DueDate { get; set; }
        [XmlIgnore]
        public bool DueDateSpecified
        {
            get
            {
                return (DueDate != null && DueDate.HasValue);
            }
        }
        [XmlElement(ElementName = "User")]
        public EFUser User { get; set; }

        [XmlElement(ElementName = "Buyer")]
        public EFClient Buyer { get; set; }
        [XmlElement(ElementName = "Seller")]
        public EFClient Seller { get; set; }



        [XmlElement(ElementName = "IsExport")]
        public bool? IsExport { get; set; }
        [XmlIgnore]
        public bool IsExportSpecified
        {
            get
            {
                return (IsExport != null && IsExport.HasValue);
            }
        }
        [XmlElement(ElementName = "ParagonBlockNumber")]
        public string ParagonBlockNumber { get; set; }
        [XmlElement(ElementName = "Currency")]
        public EFCurrency Currency { get; set; }
        [XmlElement(ElementName = "Sales")]
        public EFSales Sales { get; set; }
        [XmlElement(ElementName = "Payments")]
        public EFPayments Payments { get; set; }
        [XmlElement(ElementName = "Footer")]
        public EFFooter Footer { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "TestMode")]
        public bool TestMode { get; set; }
        [XmlElement(ElementName = "Debug")]
        public string Debug { get; set; }

    }

}
