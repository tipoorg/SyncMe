using System.ComponentModel;

namespace SyncMe.Models;

public enum SyncStatus
{
    Active, Stopped
}

public record SyncEvent(string ExternalId, string Title, string Description, Namespace Namespace, SyncSchedule Schedule, SyncAlert Alert, SyncStatus Status, DateTime Start, DateTime End);

public record NamespaceTree(Dictionary<string, List<Namespace>> Tree);

public record Namespace(int Id, string Title);

public enum SyncRepeat
{
    None, Dayly, WeekDays, EveryMonth, EveryYear, Every10Seconds
}

public record SyncSchedule(SyncRepeat Repeat);

public record SyncAlert(params SyncReminder[] Reminders);

public record SyncAlarm(string Title, Guid EventId, int DelaySeconds);

public enum SyncReminder
{
    [Description("None")]
    None = -1,
    [Description("At event time")]
    AtEventTime = 0,
    [Description("1 minutes before")]
    Before1Min = 1,
    [Description("5 minutes before")]
    Before5Min = 5,
    [Description("10 minutes before")]
    Before10Min = 10,
    [Description("15 minutes before")]
    Before15Min = 15,
    [Description("30 minutes before")]
    Before30Min = 30,
    [Description("1 hour before")]
    Before1Hour = 60,
    [Description("1 day before")]
    DayBefore = 1440,
    [Description("2 days before")]
    TwoDaysBefore = 2880,
    [Description("1 week before")]
    OneWeekBefore = 10080
}