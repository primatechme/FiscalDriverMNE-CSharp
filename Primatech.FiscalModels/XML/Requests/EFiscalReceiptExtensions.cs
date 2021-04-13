using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalModels.XML.Requests
{
    public static class EFiscalReceiptExtensions
    {
        //get price rounded at four decimals
        public static decimal CalculateTotalPrice(this EFiscalReceiptCommand model)
        {
            return model.Sales.ItemSaleRow.Sum(c => model.CalculateSalesItemPrice(c));
        }

        private static decimal CalculateSalesItemPrice(this EFiscalReceiptCommand model,EFItemSaleRow item)
        {
            var totalPrice = 0m;
            var price = item.Price;
            if (item.DiscountAmount > 0 && item.DiscountAmount <= price)
            {
                totalPrice = Math.Round((price - item.DiscountAmount) * item.Quantity, 2, MidpointRounding.AwayFromZero);
            }
            else if (item.DiscountPercentage > 0 && item.DiscountPercentage <= 100)
            {
                totalPrice = Math.Round((price * (1 - item.DiscountPercentage / 100)) * item.Quantity, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                totalPrice = Math.Round(price * item.Quantity, 2, MidpointRounding.AwayFromZero);
            }
            return Math.Round(totalPrice, 4, MidpointRounding.AwayFromZero);
        }
        //+01:00 winterTime
        //+02:00 summerTime
        public static string ToInvoiceUrl(this EFiscalReceiptCommand model, string baseUrl, string iic, string businessUnitCode, string softwareCode)
        {
            return $"{baseUrl}?iic={iic}&tin={model.Seller.IDValue}&crtd={ model.DateSend.ToLocalTimeLong()}&ord={model.DocumentNumber}&bu={businessUnitCode}&cr={model.ENUIdentifier}&sw={softwareCode}&prc={model.Payments.PaymentRow.Sum(c => c.PaymentAmount).ToString("0.00")}";
        }
    }
}
