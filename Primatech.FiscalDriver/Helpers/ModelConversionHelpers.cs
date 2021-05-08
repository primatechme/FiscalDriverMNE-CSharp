using Primatech.FiscalModels.JSON.Requests;
using Primatech.FiscalModels.XML.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Helpers
{
    public static class ModelConversionHelpers
    {
        public static EFiscalReceiptCommand ToXMLModel(this EFIReceipt receipt)
        {

            //set user
            Func<EFIUser, EFUser> SetUser = user =>
             new EFUser()
             {
                 UserName = user.UserName,
                 UserCode = user.UserCode
             };

            //set seller, buyer
            Func<EFIClient, EFClient> SetClient = client =>
            {
                if (client != null)
                {
                    return new EFClient()
                    {
                        Name = client.Name,
                        IDType = "TIN",//SellerIDType.TIN
                        IDValue = client.VATNumber,
                        Address = client.Address
                    };
                }
                return null;
            };

            //set sales
            Func<IEnumerable<EFISaleItem>, EFSales> SetSales = sales =>
            {
                EFSales salesGroup = null;
                if (sales != null)
                {
                    salesGroup = new EFSales()
                    {
                        ItemSaleRow =
                    sales.Select(item => new EFItemSaleRow()
                    {
                        ItemCode = item.ItemCode,
                        ItemName = item.ItemName,
                        Price = item.Price,
                        DiscountPercentage = item.DiscountPercentage,
                        Quantity = item.Quantity,
                        TaxRate = item.TaxRate
                    }
                ).ToList()
                    };
                }

                return salesGroup;
            };

            //set payments
            Func<IEnumerable<EFIPaymentItem>, EFPayments> SetPayments = payments =>
                new EFPayments()
                {
                    PaymentRow = payments.Select(item =>
                        new EFPaymentRow()
                        {
                            PaymentType = item.PaymentType,
                            PaymentAmount = item.Amount
                        }
                    ).ToList()
                };

            Func<IEnumerable<EFIConnectedDocument>,EFConnectedDocuments> SetConnectedDocuments = connectedDocs =>
            {
                if (connectedDocs != null)
                {
                    var list = new List<EFDocumentRow>();
                    foreach (var item in connectedDocs)
                    {
                        list.Add(new EFDocumentRow()
                        {
                            Uid=item.IKOF,
                            Type = "CORRECTIVE",
                            IssueDate = item.IssuedAt
                        });
                    }
                    return new EFConnectedDocuments()
                    {
                        DocumentRow = list
                    };
                }
                return null;
            };

            var command = new EFiscalReceiptCommand()
            {
                Uid = receipt.ReceiptUniqueIdentifier.ToString(),
                ENUIdentifier = receipt.TCRCode,
                Type = "JSON",//EFiscalReceiptCommandType
                DocumentType = receipt.ReceiptType,//"INVOICE",//EFiscalReceiptCommandDocumentType
                DocumentNumber = receipt.ReceiptNumber,
                IsNoCashReceipt = !receipt.IsCashReceipt,
                DateSend = receipt.ReceiptTime,
                User = SetUser(receipt.User),
                ConnectedDocuments = SetConnectedDocuments(receipt.ConnectedDocuments),
                Seller = SetClient(receipt.Seller),
                Buyer = SetClient(receipt.Buyer),
                Sales = SetSales(receipt.Sales),
                Payments = SetPayments(receipt.Payments)
            };
            return command;
        }

        public static EFDepositCommand ToXMLModel(this EFIDeposit deposit)
        {

            //set user
            Func<EFIUser, EFUser> SetUser = user =>
             new EFUser()
             {
                 UserName = user.UserName,
                 UserCode = user.UserCode
             };

            var command = new EFDepositCommand()
            {
                Uid = deposit.Uid.ToString(),
                ENUIdentifier = deposit.TCRCode,
                Type = "JSON",//EFiscalReceiptCommandType
                DateSend = deposit.Time,
                User = SetUser(deposit.User),
                Amount=deposit.Amount,
                DepositType=deposit.DepositType
            };
            
            return command;
        }
    }
}
