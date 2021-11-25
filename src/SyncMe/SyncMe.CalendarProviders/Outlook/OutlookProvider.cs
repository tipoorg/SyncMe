using Azure.Identity;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncMe.CalendarProviders.Outlook
{
    public class OutlookProvider
    {
        private readonly GraphServiceClient _graphClient;
        private readonly string _email;
        public OutlookProvider(GraphServiceClient graphClient, string email)
        {
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
            while (true)
            {
                foreach (var @event in events)
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
