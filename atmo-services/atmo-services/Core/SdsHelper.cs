using OSIsoft.Data;
using OSIsoft.Data.Reflection;
using OSIsoft.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atmo_services
{
    public static class SdsHelper
    {
        const string accountId = "[OSC Account ID]";
        const string namespaceId = "atmo";
        const string resource = "https://dat-b.osisoft.com";
        const string clientId = "4a7d2558-a011-4af2-8554-4cf99b0c513f";

        private static AuthenticationHandler securityHandler = new AuthenticationHandler(new Uri(resource), clientId, "kyydDDqLfO4xiaC6CmgtoaXUT0HbjjryV1NtViMrm8c=");
        private static SdsService sdsService = new SdsService(new Uri(resource), new JsonFormatter(), securityHandler);
        public static ISdsMetadataService metadata = sdsService.GetMetadataService(accountId, namespaceId);
        public static ISdsDataService dataService = sdsService.GetDataService(accountId, namespaceId);

        public static async Task<SdsStream> GetOrCreateTypeAndStreamAsync<T>(string id, string description)
        {
            // Stream retrieval call
            var streams = await SdsHelper.metadata.GetStreamsAsync(id);
            var stream = streams.FirstOrDefault();

            if (stream == null)
            {
                SdsType type = SdsTypeBuilder.CreateSdsType<T>();
                type.Id = type.Name;
                await SdsHelper.metadata.GetOrCreateTypeAsync(type);

                stream = new SdsStream
                {
                    Id = id,
                    Name = id,
                    TypeId = type.Id,
                    Description = description
                };

                await SdsHelper.metadata.CreateOrUpdateStreamAsync(stream);
            }

            return stream;
        }
    }
}
