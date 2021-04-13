using Primatech.FiscalModels.XML.Base;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Primatech.FiscalModels.XML.Requests
{
    [XmlRoot(ElementName = "NonFiscalCommand")]
    public class NonFiscalEFCommand : BaseEFCommand
    {
        [XmlElement(ElementName = "NonFiscalRows")]
        public NonFiscalTextRowEl NonFiscalRows { get; set; }
    }

    [XmlRoot(ElementName = "NonFiscalRows")]
    public class NonFiscalTextRowEl
    {
        [XmlElement(ElementName = "NonFiscalText")]
        public List<EFNonFiscalTextRow> NonFiscalTextRows { get; set; }
    }

    [XmlRoot(ElementName = "NonFiscalText")]
    public class EFNonFiscalTextRow
    {
        [XmlElement(ElementName = "Message")]
        public string Message { get; set; }
    }
}
