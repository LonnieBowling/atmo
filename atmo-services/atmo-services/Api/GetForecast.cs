using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace atmo_services
{
    public static class GetForecast
    {
        [FunctionName("GetForecast")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var key = "locationId";
            if (req.Query.ContainsKey(key))
            {

                var data = await Api.GetForecastAsync(req.Query[key]);

                return data != null
                    ? (ActionResult)new OkObjectResult(data)
                    : new BadRequestObjectResult("unable to complete request");
            }
            else
            {
                return new BadRequestObjectResult("locationId not contained in request");
            }
        }
    }
}
