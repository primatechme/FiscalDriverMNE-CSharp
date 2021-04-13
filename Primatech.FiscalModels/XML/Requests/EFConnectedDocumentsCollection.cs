using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "ConnectedDocuments")]
    public class EFConnectedDocuments
    {
        [XmlElement(ElementName = "DocumentRow")]
        public List<EFDocumentRow> DocumentRow { get; set; }
    }

    [XmlRoot(ElementName = "DocumentRow")]
    public class EFDocumentRow
    {

        [XmlElement(ElementName = "UID")]
        public string Uid { get; set; }
        [XmlElement(ElementName = "IssueDate")]
        public DateTime IssueDate { get; set; }
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "Total")]
        public decimal Total { get; set; }
    }
}
