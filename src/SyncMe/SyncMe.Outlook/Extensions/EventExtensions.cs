using SyncMe.Models;
using Microsoft.Graph;

namespace SyncMe.Outlook.Extensions;

public static class EventExtensions
{
    public static SyncEvent ToSyncEvent(this Event e, string? username = null) => new SyncEvent
    {
        ExternalId = e.Id,
        ExternalEmail = username,
        Title = e.Subject,
        Description = e.Body.Content,
        NamespaceKey = Namespace.Root.Key,
        Repeat = SyncRepeat.None,
        Reminder = SyncReminder.AtEventTime,
        Status = SyncStatus.Active,
        Start = DateTime.Parse(e.Start.DateTime).ToLocalTime(),
        End = DateTime.Parse(e.End.DateTime).ToLocalTime()
    };
}
