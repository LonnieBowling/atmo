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
    public static class WeatherStationCollector
    {
        [FunctionName("weather-station-collector")]
        public static void Run([TimerTrigger("30 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Starting weather station write: {DateTime.Now}");
            
            //Read data from weather station
            var data = GetDataAsync().Result;

            //Convert data to Weather Station type
            var wsData = AmbientWeather.Convert(data);

            //Write data to OCS
            var results = WriteDataAsync("napa.pluto.snapshot", wsData).Result;

            log.LogInformation($"Completed weather station write: {DateTime.Now}");
        }

        private static async Task<JObject> GetDataAsync()
        {
            try
            {
                string request = AmbientWeather.Url;
                using (var httpClient = new HttpClient())
                {
                    var results = await httpClient.GetStringAsync(request);
                    var data = (JObject)JArray.Parse(results).First;
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task<bool> WriteDataAsync(string id, WeatherStation data)
        {
            try
            {
                var description = "Snapshot values for ${id}";
                var stream = await SdsHelper.GetOrCreateTypeAndStreamAsync<WeatherStation>(id, description);

                //Write data
                if(stream != null)
                {
                    await SdsHelper.dataService.UpdateValueAsync<WeatherStation>(stream.Id, data);
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
