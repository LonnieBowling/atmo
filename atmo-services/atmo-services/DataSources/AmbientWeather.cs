using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace atmo_services
{
    public static class AmbientWeather
    {
        private static string applicationKey = "e35aea0bfc9a410cad6caed481f4ccee63059bd61ab0486e96d19c942d201aa2";
        private static string apiKey = "[Ambient API Key]";
        private static string baseUrl = "https://api.ambientweather.net/v1";

        public static string Url
        {
            get
            {
                return $"{baseUrl}/devices?applicationKey={applicationKey}&apiKey={apiKey}";
            }
        }

        public static WeatherStation Convert(JObject data)
        {
            var ws = new WeatherStation();
            var values = (JObject)data["lastData"];
            ws.Time = DateTime.Parse(values["date"].Value<string>());
            ws.Temperature = values["tempf"].Value<double>();
            ws.TemperatureIndoor = values["tempinf"].Value<double>();
            return ws;
        }

    }
}
