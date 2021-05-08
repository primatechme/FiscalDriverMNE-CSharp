using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primatech.FiscalDriver.Helpers
{
    public static class JsonHelpers
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToJson(this object obj, Formatting formatting)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static T ToModel<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
    }
}
