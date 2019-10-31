using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace atmo_services
{
    public static class Darksky
    {

        private static string applicationKey = "[Dark Sky Application Key]";
        private static string baseUrl = "https://api.darksky.net/forecast";

        public static string GetUrl(double latitude, double longitude)
        {
            return $"{baseUrl}/{applicationKey}/{latitude},{longitude}";
        }

        public static Forecast Convert(JObject data)
        {
            var fc = new Forecast();
            var current = new ForecastCurrent();
            var cData = (JObject)data["currently"];
            current.Time = UnixSecondsToDateTime(cData["time"].Value<long>());
            current.Summary = cData["summary"].Value<string>();
            current.Icon = cData["icon"].Value<string>();

            var dData = (JObject)data["daily"];
            current.SummaryDaily = dData["summary"].Value<string>();
            fc.Current = current;

            var daily = new List<ForecastDaily>();

            foreach (var item in dData["data"])
            {
                var d = new ForecastDaily();
                d.Time = UnixSecondsToDateTime(item["time"].Value<long>());
                d.Summary = item["summary"].Value<string>();
                d.Icon = item["icon"].Value<string>();
                d.TemperatureLow = item["temperatureMin"].Value<double>();
                d.TemperatureHigh = item["temperatureMax"].Value<double>();

                daily.Add(d);
            }
            fc.Daily = daily;

            return fc;
        }

        public static DateTime UnixSecondsToDateTime(long value)
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(value);
        }

    }
}
