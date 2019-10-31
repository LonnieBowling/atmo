using OSIsoft.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace atmo_services
{
    public class WeatherStation
    {
        [SdsMember(IsKey = true)]
        public DateTime Time { get; set; } //"date": "2019-10-13T19:10:00.000Z"
        [SdsMember(Uom = "degree Fahrenheit")]
        public double? Temperature { get; set; } //"tempf": 74.3,
        [SdsMember(Uom = "degree Fahrenheit")]
        public double? TemperatureIndoor { get; set; } //"tempinf": 68.2,

    }
}
