using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "Sales")]
    public class EFSales
    {
        [XmlElement(ElementName = "ItemSaleRow")]
        public List<EFItemSaleRow> ItemSaleRow { get; set; }
    }

    [XmlRoot(ElementName = "ItemSaleRow")]
    public class EFItemSaleRow
    {
        [XmlElement(ElementName = "ItemCode")]
        public string ItemCode { get; set; }
        [XmlElement(ElementName = "ItemName")]
        public string ItemName { get; set; }
        [XmlElement(ElementName = "Price")]
        public decimal Price { get; set; }
        [XmlElement(ElementName = "DiscountPercentage")]
        public decimal DiscountPercentage { get; set; }
        [XmlElement(ElementName = "DiscountAmount")]
        public decimal DiscountAmount { get; set; }
        [XmlElement(ElementName = "Quantity")]
        public decimal Quantity { get; set; }
        [XmlElement(ElementName = "TaxRate")]
        public decimal TaxRate { get; set; }
        [XmlElement(ElementName = "IsTaxFree")]
        public bool? IsTaxFree { get; set; }
        [XmlIgnore]
        public bool IsTaxFreeSpecified
        {
            get
            {
                return (IsTaxFree != null && IsTaxFree.HasValue);
            }
        }
        [XmlElement(ElementName = "TaxFreeReason")]
        public string TaxFreeReason { get; set; }
        [XmlElement(ElementName = "UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        public string UnitOfMeasureOrDefault()
        {
            if (String.IsNullOrEmpty(UnitOfMeasure))
                return "piece";
            return UnitOfMeasure;
        }
    }

}
