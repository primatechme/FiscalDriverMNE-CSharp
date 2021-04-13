using Primatech.FiscalDriver.Helpers;
using Primatech.FiscalDriver.Infrastructure.Builders;
using Primatech.FiscalModels.JSON.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Infrastructure
{
    public class EFiscalManagerSimplified
    {

        public static EFIReceipt CreateReceipt()
        {
            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
                .SetTCRCode("**********")
                .SetDates(DateTime.Now, null)
                //.SetIsCash(false)
                .SetJsonResponse()
                .SetUser("Marko Markovic", "**********")
                .SetSeller("Seller Company Name", "01234567", "Some Seller Address")
                .SetBuyer("Buyer Company Name", "07654321", "Some Buyer Address")
                .AddSaleItem("12", "Coca Cola 0.25l", 2, 2.20m, 21m)
                .AddSaleItem("22", "Fanta 0.5l", 2, 2.20m, 21m, 10)
                .AddSaleItem("19", "Sir Gauda", 2, 2.20m, 7m, 0)
                //.SetTotalDiscount(10)
                .AddPayment("CASH", 120m)
                .CalculateTotalAmount("CASH");

            return receipt;
        }

        public static EFIDeposit CreateDeposit()
        {
            return DepositBuilder.Build("**********", 25m)
                        .SetTime(DateTime.Now)
                        .SetAmount(25m)
                        .SetUser("Marko Markovic", "**********");
        }

        public const string BASE_URL = "****";
        public const string TOKEN = "****";
        public async Task<object> TestReceipt()
        {
            var service=new FiscalApiService(BASE_URL, TOKEN);

            var receiptXML = ReceiptBuilder.Build(Guid.NewGuid(), 1)
               .SetTCRCode("**********")
               .SetDates(DateTime.Now, null)
               //.SetIsCash(false)
               .SetJsonResponse()
               .SetUser("Marko Markovic", "**********")
               .SetSeller("Seller Company Name", "01234567", "Some Seller Address")
               .SetBuyer("Buyer Company Name", "07654321", "Some Buyer Address")
               .AddSaleItem("12", "Coca Cola 0.25l", 2, 2.20m, 21m)
               .AddSaleItem("22", "Fanta 0.5l", 2, 2.20m, 21m, 10)
               .AddSaleItem("19", "Sir Gauda", 2, 2.20m, 7m, 0)
               //.SetTotalDiscount(10)
               .AddPayment("CASH", 120m)
               .CalculateTotalAmount("CASH")
               .ToXMLModel();

            return await service.CreateReceipt(receiptXML);
        }

        public async Task<object> TestReceiptV2()
        {
            //generisemo testne podatke
            var buyer = new
            {
                CompanyName = "Buyer Company Name",
                VATNumber = "07654321",
                Address = "Some Buyer Address"
            };

            var sales =
            new[]{
                new{ ItemCode="12", ItemName="Coca Cola 0.25l", Quantity=2, Price=2.20m, TaxRate=21m,Discount=0},
                new{ ItemCode="22", ItemName="Fanta 0.5l", Quantity=2, Price=2.20m, TaxRate=21m,Discount=10},
                new{ ItemCode="19", ItemName="Sir Gauda", Quantity=1, Price=2.20m, TaxRate=7m,Discount=0}
            };

            var service = new FiscalApiService(BASE_URL, TOKEN);

            var receipt = ReceiptBuilder.Build(Guid.NewGuid(), 1)
               .SetTCRCode("**********")
               .SetDates(DateTime.Now, null)
               .SetUser("Marko Markovic", "**********")
               .SetSeller("Seller Company Name", "01234567", "Some Seller Address");

            if (buyer != null)
            {
                receipt.SetBuyer(buyer.CompanyName, buyer.VATNumber, buyer.Address);
            }

            foreach(var saleRow in sales)
            {
                receipt.AddSaleItem(saleRow.ItemCode, saleRow.ItemName, saleRow.Quantity, saleRow.Price, saleRow.TaxRate);
            }

            receipt.AddPayment("CASH", 120m);

            return await service.CreateReceipt(receipt.ToXMLModel());
        }

        public async Task<object> TestDeposit()
        {
            var service = new FiscalApiService(BASE_URL, TOKEN);

            var depositXML = DepositBuilder.Build("**********", 25m)
                       //.SetTime(DateTime.Now)
                       //.SetAmount(25m)
                       .SetUser("Marko Markovic", "**********")
                       .ToXMLModel();

            return await service.CreateDeposit(depositXML);
        }

    }
}
