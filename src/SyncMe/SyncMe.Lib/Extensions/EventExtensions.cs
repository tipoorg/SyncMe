using SyncMe.Models;
using Microsoft.Graph;


namespace SyncMe.Extensions
{
    internal static class EventExtensions
    {
        public static SyncEvent ToSyncEvent(this Event e, string username = null) => new SyncEvent
        {
            ExternalId = e.Id,
            ExternalEmail = username,
            Title = e.Subject,
            Description = e.Body.Content,
            Namespace = new Namespace { Title = "" },
            Schedule = new SyncSchedule { Repeat = SyncRepeat.None },
            Alert = new SyncAlert { Reminder = SyncReminder.AtEventTime },
            Status = SyncStatus.Active,
            Start = DateTime.Parse(e.Start.DateTime).ToLocalTime(),
            End = DateTime.Parse(e.End.DateTime).ToLocalTime()
        };
    }
}
