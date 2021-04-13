using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Primatech.FiscalModels.XML.Requests;

namespace Primatech.FiscalModels.JSON.Requests
{
    public class EFIReceipt
    {
        public Guid ReceiptUniqueIdentifier { get; set; }
        public string TCRCode { get; set; }
        public string ReceiptType { get; set; }
        public int ReceiptNumber { get; set; }
        public DateTime ReceiptTime { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCashReceipt { get; set; }
        public EFIUser User { get; set; }
        public EFIClient Seller { get; set; }
        public EFIClient Buyer { get; set; }
        public IList<EFIConnectedDocument> ConnectedDocuments { get; set; }
        public IList<EFISaleItem> Sales { get; set; }
        public IList<EFIPaymentItem> Payments { get; set; }

       
    }

    public class EFIConnectedDocument
    {
        public string IKOF { get; set; }
        public DateTime IssuedAt { get; set; }
        //public string Type { get; set; }
        public decimal Amount { get; set; }
    }
}
