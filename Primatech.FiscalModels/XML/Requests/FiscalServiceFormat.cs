using System;

namespace Primatech.FiscalModels.XML.Requests
{
    public static class FiscalServiceFormat
    {
        public const string DATE_FORMAT_LONG_EU = "yyyy-MM-ddTHH:mm:sszzz";

        public static string ToLocalTimeLong(this DateTime time)
        {
            DateTime cstTime = TimeZoneInfo.ConvertTimeToUtc(time);
            var newDate = cstTime.AddHours(2);
            return newDate.ToString("yyyy-MM-ddTHH:mm:ss") + "+02:00";
        }
    }
}
