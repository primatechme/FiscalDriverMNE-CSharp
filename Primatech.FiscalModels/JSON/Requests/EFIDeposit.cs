using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalModels.JSON.Requests
{
    public class EFIDeposit
    {
        public string Uid { get; set; }
        public string TCRCode { get; set; }
        public string DepositType { get; set; }
        public DateTime Time { get; set; }
        public decimal Amount { get; set; }

        public EFIUser User { get; set; }
    }

    public enum EFIDepositTypeEnum
    {
        INITIAL,
        WITHDRAW
    }
}
