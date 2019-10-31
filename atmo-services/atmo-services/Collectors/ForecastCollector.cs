using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OSIsoft.Data;
using OSIsoft.Data.Reflection;
using OSIsoft.Identity;

namespace atmo_services
{
    public static class ForecastCollector
    {

        [FunctionName("forecast-collector")]
        public static void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Starting forecast write: {DateTime.Now}");
            
            //Read data from Dark Sky
            double latitude = 37.8267;
            double longitude = -122.4233;
            var data = GetDataAsync(latitude, longitude).Result;

            //Convert data to forcast type
            var fcData = Darksky.Convert(data);

            //Write data to OCS
            var results = WriteDataAsync("napa.pluto", fcData).Result;

            log.LogInformation($"Completed forecast write: {DateTime.Now}");
        }

        private static async Task<JObject> GetDataAsync(double latitude, double longitude)
        {
            try
            {
                string request = Darksky.GetUrl(latitude,longitude);
                using (var httpClient = new HttpClient())
                {
                    var results = await httpClient.GetStringAsync(request);
                    var data = JObject.Parse(results);
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<bool> WriteDataAsync(string baseId, Forecast data)
        {
            try
            {
                // Write current value
                var id = $"{baseId}.forecast.current";
                var description = $"Forecast current value for {baseId}";
                var stream = await SdsHelper.GetOrCreateTypeAndStreamAsync<ForecastCurrent>(id, description);
                
                if(stream != null)
                {
                    await SdsHelper.dataService.UpdateValueAsync<ForecastCurrent>(stream.Id, data.Current);
                }

                id = $"{baseId}.forecast.daily";
                description = $"Forecast daily value for {baseId}";
                stream = await SdsHelper.GetOrCreateTypeAndStreamAsync<ForecastDaily>(id, description);
                if(stream != null)
                {
                    foreach (var item in data.Daily)
                    {
                        await SdsHelper.dataService.UpdateValueAsync(stream.Id, item);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }  

        }


    }
}
