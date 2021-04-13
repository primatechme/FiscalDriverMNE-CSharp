using Primatech.FiscalModels.JSON.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Infrastructure.Builders
{
    public static class DepositBuilder
    {
        public static EFIDeposit Build(string TCRCode)
        {
            return new EFIDeposit
            {
                Uid = Guid.NewGuid().ToString(),
                TCRCode = TCRCode,
                Time = DateTime.Now
            };
        }

        public static EFIDeposit Build(string TCRCode,decimal amount)
        {
            var depositType = amount >= 0 ? EFIDepositTypeEnum.INITIAL : EFIDepositTypeEnum.WITHDRAW;
            return new EFIDeposit
            {
                Uid = Guid.NewGuid().ToString(),
                TCRCode = TCRCode,
                DepositType = ""+depositType,
                Time = DateTime.Now,
                Amount = Math.Abs(amount)
            };
        }

        public static EFIDeposit Build(Guid depositIdentifier, string TCRCode, DateTime depositTime, decimal amount)
        {
            var depositType = amount >= 0 ? EFIDepositTypeEnum.INITIAL : EFIDepositTypeEnum.WITHDRAW;
            return new EFIDeposit
            {
                Uid = depositIdentifier.ToString(),
                TCRCode = TCRCode,
                DepositType = "" + depositType,
                Time = depositTime,
                Amount = Math.Abs(amount)
            };
        }

        public static EFIDeposit SetUser(this EFIDeposit deposit, string userName, string userCode)
        {
            deposit.User = new EFIUser
            {
                UserName = userName,
                UserCode = userCode
            };
            return deposit;
        }

        public static EFIDeposit SetTime(this EFIDeposit deposit, DateTime time)
        {
            deposit.Time = time;
            return deposit;
        }

        public static EFIDeposit SetDepositType(this EFIDeposit deposit, EFIDepositTypeEnum depositType)
        {
            deposit.DepositType = "" + depositType;
            return deposit;
        }

        public static EFIDeposit SetAmount(this EFIDeposit deposit, decimal amount)
        {
            var depositType = amount >= 0 ? EFIDepositTypeEnum.INITIAL : EFIDepositTypeEnum.WITHDRAW;
            deposit.DepositType = "" + depositType;
            deposit.Amount = Math.Abs(amount);
            return deposit;
        }
    }

    
}
