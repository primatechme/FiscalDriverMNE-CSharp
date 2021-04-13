using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Models
{
    public class ExtendedHttpClient : HttpClient
    {
        public string BaseUrl { get; set; }
        public string RequestUri { get; set; }


        public ExtendedHttpClient() { }
        public ExtendedHttpClient(string url) : base()
        {
            BaseUrl = url;
            RequestUri = url;
        }
    }
}
