using OSIsoft.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace atmo_services
{
    public class Forecast
    {
        public ForecastCurrent Current { get; set; }
        public List<ForecastDaily> Daily { get; set; }
    }
    public class ForecastCurrent
    {
        [SdsMember(IsKey = true)]
        public DateTime Time { get; set; } // "time": 1571009642,
        public string Summary { get; set; } // "summary": "Clear",
        public string Icon { get; set; } // "icon": "clear-day",
        public string SummaryDaily { get; set; }

    }

    public class ForecastDaily
    {
        [SdsMember(IsKey = true)]
        public DateTime Time { get; set; } // "time": 1571009642,
        public string Summary { get; set; } // "summary": "Clear",
        public string Icon { get; set; } // "icon": "clear-day",
        [SdsMember(Uom = "degree Fahrenheit")]
        public double? TemperatureLow { get; set; } //  "temperatureMin": 54.36,
        [SdsMember(Uom = "degree Fahrenheit")]
        public double? TemperatureHigh { get; set; } // "temperatureMax": 65.36,

    }
}
