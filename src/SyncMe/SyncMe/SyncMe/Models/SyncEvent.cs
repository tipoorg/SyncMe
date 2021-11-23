using System.ComponentModel;

namespace SyncMe.Models;

public enum SyncStatus
{
    Active, Stopped
}

public record SyncEvent(string Title, string Description, Namespace Namespace, SyncSchedule Schedule, SyncAlert Alert, SyncStatus Status);

public record NamespaceTree(ILookup<Namespace, Namespace> Tree);

public record Namespace(int Id, string Title);

public enum SyncRepeat
{
    None, Dayly, WeekDays, EveryMonth, EveryYear, Every10Seconds
}

public record SyncSchedule(SyncRepeat Repeat, int? Times);

public record SyncAlert(SyncReminder[] Reminders);

public enum SyncReminder
{
    [Description("None")]
    None,
    [Description("At event time")]
    AtEventTime,
    [Description("1 minutes before")]
    Before1Min,
    [Description("5 minutes before")]
    Before5Min,
    [Description("10 minutes before")]
    Before10Min,
    [Description("15 minutes before")]
    Before15Min,
    [Description("30 minutes before")]
    Before30Min,
    [Description("1 hour before")]
    Before1Hour,
    [Description("1 day before")]
    DayBefore,
    [Description("2 days before")]
    TwoDaysBefore,
    [Description("1 week before")]
    OneWeekBefore
}