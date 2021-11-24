using System.ComponentModel;

namespace SyncMe.Models;

public enum SyncStatus
{
    Active, Stopped
}

public class SyncEvent
{
    public string ExternalId { get; set; }
    public string? ExternalEmail { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Namespace Namespace { get; set; }
    public SyncSchedule Schedule { get; set; }
    public SyncAlert Alert { get; set; }
    public SyncStatus Status { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}

public record NamespaceTree(Dictionary<string, List<Namespace>> Tree);

public class Namespace
{
    public string Title { get; set; }
    public Guid Id { get; init; }
}

public enum SyncRepeat
{
    [Description("None")]
    None,
    [Description("Daily")]
    Dayly,
    [Description("Week Days")]
    WeekDays,
    [Description("Eveery Month")]
    EveryMonth, 
    [Description("Every Year")]
    EveryYear,
    [Description("Every 10 seconds")]
    Every10Seconds
}

public class SyncSchedule
{
    public SyncRepeat Repeat { get; set; }
}

public class SyncAlert
{
    public SyncReminder[] Reminders { get; set; } = new SyncReminder[1];
}

public record SyncAlarm(string Title, Guid EventId, int DelaySeconds);

public enum SyncReminder
{
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