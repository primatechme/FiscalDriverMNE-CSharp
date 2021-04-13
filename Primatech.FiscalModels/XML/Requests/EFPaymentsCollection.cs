using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{

    [XmlRoot(ElementName = "Payments")]
    public class EFPayments
    {
        [XmlElement(ElementName = "PaymentRow")]
        public List<EFPaymentRow> PaymentRow { get; set; }
    }

    [XmlRoot(ElementName = "PaymentRow")]
    public class EFPaymentRow
    {
        [XmlElement(ElementName = "PaymentAmount")]
        public decimal PaymentAmount { get; set; }
        [XmlElement(ElementName = "PaymentType")]
        public string PaymentType { get; set; }
    }
}
