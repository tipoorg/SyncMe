﻿using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarProviders.Abstractions
{
    public interface ICalendarProvider
    {
        Task<List<Event>> GetEventsAsync();
    }
}
