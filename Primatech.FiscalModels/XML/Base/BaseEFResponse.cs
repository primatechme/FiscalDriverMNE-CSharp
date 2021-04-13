using Primatech.FiscalModels.XML.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalModels.XML.Base
{
    public class BaseEFResponse
    {
        public bool IsSucccess { get; set; }
        public EFErrorResponse Error { get; set; }
    }
}
