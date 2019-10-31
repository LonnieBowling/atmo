using Newtonsoft.Json.Linq;
using OSIsoft.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace atmo_services
{
    public static class Api
    {
        public static async Task<JObject> GetSnapshotAsync(string locationId)
        {
            try
            {
                //get data from OCS
                var data = await SdsHelper.dataService.GetLastValueAsync<WeatherStation>($"{locationId}.snapshot");

                //Converting data to Json
                return await ConvertToJObjectAsync(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<JObject> GetForecastAsync(string locationId)
        {
            try
            {
                var results = new JObject();

                //current forecast

                //get data from OCS
                var cdata = await SdsHelper.dataService.GetLastValueAsync<ForecastCurrent>($"{locationId}.forecast.current");
                //convert to Json
                results["current"] = await ConvertToJObjectAsync(cdata);

                //daily forecast

                //get data from OCS
                var dData = await SdsHelper.dataService.GetWindowValuesAsync<ForecastDaily>($"{locationId}.forecast.daily", cdata.Time.ToString(), cdata.Time.AddDays(8).ToString(), SdsBoundaryType.Exact);
                //convert to Json
                var jDaily = new JArray();
                foreach (var item in dData)
                {
                    jDaily.Add(await ConvertToJObjectAsync(item));
                }

                results["daily"] = jDaily;


                //Converting data to Json
                return results;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<JObject> ConvertToJObjectAsync(Object data)
        {
            try
            {
                var properties = data.GetType().GetProperties();

                var results = new JObject();
                foreach (var prop in properties)
                {
                    if (prop.Name == "Time")
                    {
                        results[FirstCharToLowerCast(prop.Name)] = (DateTime)prop.GetValue(data);
                    }else if(prop.PropertyType.Name == "String")
                    {
                        var jVal = new JObject();
                        jVal["value"] = prop.GetValue(data).ToString();

                        results[FirstCharToLowerCast(prop.Name)] = jVal;
                    }
                    else
                    {
                        var jVal = new JObject();
                        jVal["value"] = Convert.ToDouble(prop.GetValue(data));

                        //Uom
                        var attributes = prop.GetCustomAttributes(false);
                        foreach (SdsMemberAttribute attr in attributes)
                        {
                            if (attr.Uom != null)
                            {
                                var uom = await SdsHelper.metadata.GetUomAsync(attr.Uom);
                                jVal["uom"] = uom.Abbreviation;
                            }
                        }

                        results[FirstCharToLowerCast(prop.Name)] = jVal;
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
            private static string FirstCharToLowerCast(string value)
        {
            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}
