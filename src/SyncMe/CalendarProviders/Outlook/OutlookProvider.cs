using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncMe.Providers.OutlookProvider
{ 
    public class OutlookProvider
    {
        private readonly GraphServiceClient? _graphClient;
        private readonly string _email;
        public OutlookProvider(GraphServiceClient graphClient, string email)
        {
            //var scopes = new[] { "Calendars.Read" };

            //var tenantId = "common";
            //var clientId = "904b52b5-a7ad-4ad5-b7e5-23160e0800e1";
            ////Can be used later
            ////var clientSecret = "Gve7Q~PMZJwmfrDSn7FBCxuGHu0hFNhMybR0W";

            //var options = new TokenCredentialOptions
            //{
            //    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            //};

            //Func<DeviceCodeInfo, CancellationToken, Task> callback = (code, cancellation) => {
            //    Console.WriteLine(code.Message);
            //    return Task.FromResult(0);
            //};

            //var deviceCodeCredential = new DeviceCodeCredential(
            //    callback, tenantId, clientId, options);

            //_graphClient = new GraphServiceClient(deviceCodeCredential, scopes);
            _graphClient = graphClient;
            _email = email;
        }
        public async Task<List<Event>> GetEventsAsync()
        {
            var events = await _graphClient.Users[_email].Calendar.Events.Request().GetAsync();
            var now = DateTime.Now;
            var result = new List<Event>();

            var nextRequest = events.NextPageRequest;
            var isLastBatch = false;
            while(true)
            {
                foreach(var @event in events)
                {
                    if (DateTime.Parse(@event.Start.DateTime) < now)
                    {
                        isLastBatch = true;
                        break;
                    }
                    result.Add(@event);
                }

                if (nextRequest is null || isLastBatch) break;
                events = await nextRequest.GetAsync();
            }

            return result;
        }
    }
}
