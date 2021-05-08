using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalModels.JSON.Requests
{
    public enum EFIPaymentTypeEnum
    {
        //cash
        BANKNOTE,
        CARD,
        ORDER,
        [Description("OTHER-CASH")]
        OTHER_CASH,
        //non-cash
        BUSINESSCARD,
        COMPANY,
        ACCOUNT,
        OTHER,
        ADVANCE,
        //others
        SVOUCHER,
        FACTORING
    }

    public static class EFIPaymentTypeEnumExtensions
    {
        public static string ToDecriptionString(this EFIPaymentTypeEnum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : ""+val;
        }
    }
}
