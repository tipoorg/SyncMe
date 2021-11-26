using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncMe.CalendarProviders.Abstractions
{
    public interface ICalendarProvider
    {
        Task<List<Event>> GetEventsAsync();
    }
}
