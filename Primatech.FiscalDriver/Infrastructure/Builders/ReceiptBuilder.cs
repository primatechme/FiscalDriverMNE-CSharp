using Primatech.FiscalModels.JSON.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Infrastructure.Builders
{
    public static class ReceiptBuilder
    {
        public static EFIReceipt Build(Guid receiptIdentifier, string TCRCode, int receiptNumber, DateTime receiptTime, DateTime? dueDate, bool isCashReceipt)
        {
            return new EFIReceipt
            {
                ReceiptType="INVOICE",
                ReceiptUniqueIdentifier = receiptIdentifier,
                TCRCode = TCRCode,
                ReceiptNumber = receiptNumber,
                DueDate = dueDate,
                IsCashReceipt = isCashReceipt
            };
        }

        public static EFIReceipt Build(Guid receiptIdentifier, string TCRCode, int receiptNumber)
        {
            return new EFIReceipt
            {
                ReceiptType = "INVOICE",
                ReceiptUniqueIdentifier = receiptIdentifier,
                TCRCode = TCRCode
            };
        }

        public static EFIReceipt Build(Guid receiptIdentifier, int receiptNumber)
        {
            return new EFIReceipt
            {
                ReceiptType = "INVOICE",
                ReceiptUniqueIdentifier = receiptIdentifier,
                ReceiptNumber = receiptNumber
            };
        }

        public static EFIReceipt SetTCRCode(this EFIReceipt receipt, string TCRCode)
        {
            receipt.TCRCode = TCRCode;
            return receipt;
        }

        public static EFIReceipt SetIsCash(this EFIReceipt receipt, bool isCashReceipt)
        {
            receipt.IsCashReceipt = isCashReceipt;
            return receipt;
        }

        public static EFIReceipt SetCorrectiveInvoice(this EFIReceipt receipt)
        {
            receipt.ReceiptType = "CORRECTIVE_INVOICE";
            return receipt;
        }

        public static EFIReceipt SetDates(this EFIReceipt receipt, DateTime receiptTime, DateTime? dueDate)
        {
            receipt.ReceiptTime = receiptTime;
            receipt.DueDate = dueDate;
            return receipt;
        }

        public static EFIReceipt SetUser(this EFIReceipt receipt, string userName, string userCode)
        {
            receipt.User = new EFIUser
            {
                UserName = userName,
                UserCode = userCode
            };
            return receipt;
        }

        public static EFIReceipt SetSeller(this EFIReceipt receipt, string name, string VATNumber, string address)
        {
            receipt.Seller = new EFIClient
            {
                Name = name,
                VATNumber = VATNumber,
                Address = address
            };
            return receipt;
        }

        public static EFIReceipt SetBuyer(this EFIReceipt receipt, string name, string VATNumber, string address)
        {
            receipt.Buyer = new EFIClient
            {
                Name = name,
                VATNumber = VATNumber,
                Address = address
            };
            return receipt;
        }

        public static EFIReceipt AddSaleItem(this EFIReceipt receipt, string itemCode, string itemName, decimal quantity, decimal price, decimal taxRate, decimal discountPercentage)
        {
            if (receipt.Sales == null)
            {
                receipt.Sales = new List<EFISaleItem>();
            }
            receipt.Sales.Add(
                new EFISaleItem
                {
                    ItemCode = itemCode,
                    ItemName = itemName,
                    Quantity = quantity,
                    Price = price,
                    TaxRate = taxRate,
                    DiscountPercentage = discountPercentage

                });
            return receipt;
        }

        public static EFIReceipt AddSaleItem(this EFIReceipt receipt, string itemCode, string itemName, decimal quantity, decimal price, decimal taxRate)
        {
            return AddSaleItem(receipt, itemCode, itemName, quantity, price, taxRate,0);
        }

        public static EFIReceipt SetTotalDiscount(this EFIReceipt receipt, decimal discountPercentage)
        {
            if (receipt.Sales == null)
            {
                receipt.Sales = new List<EFISaleItem>();
            }
            receipt.Sales.Select(item => item.DiscountPercentage = discountPercentage);
            return receipt;
        }


        public static EFIReceipt AddPayment(this EFIReceipt receipt, string paymentType, decimal amount)
        {
            if (receipt.Payments == null)
            {
                receipt.Payments = new List<EFIPaymentItem>();
            }
            receipt.Payments.Add(new EFIPaymentItem
            {
                PaymentType = paymentType,
                Amount = amount,
            });
            return receipt;
        }

        public static EFIReceipt CalculateTotalAmount(this EFIReceipt receipt, string paymentType)
        {
            receipt.Payments = new List<EFIPaymentItem>();
            receipt.Payments.Add(new EFIPaymentItem
            {
                PaymentType = paymentType,
                Amount = receipt.Sales.Sum(item => item.Price * item.Quantity * (1 - item.DiscountPercentage / 100))
            });
            return receipt;
        }

        public static EFIReceipt SetJsonResponse(this EFIReceipt receipt)
        {
            //receipt.ResponseType = "JSON";
            return receipt;
        }

        public static EFIReceipt SetXMLResponse(this EFIReceipt receipt)
        {
            //receipt.ResponseType = "XML";
            return receipt;
        }

        public static EFIReceipt SetTextResponse(this EFIReceipt receipt)
        {
            //receipt.ResponseType = "TXT";
            return receipt;
        }

    }
}
