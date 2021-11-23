namespace SyncMe.Models;

public enum Status
{
    Active, Stopped
}

public record SyncEvent(int Id, string Title, string Description, Namespace Namespace, Schedule Schedule, Alert Alert, Status Status);

public record NamespaceTree(ILookup<Namespace, Namespace> Tree);

public record Namespace(int Id, string Title);

public enum Repeat
{
    None, Dayly, WeekDays, EveryMonth, EveryYear, Every10Seconds
}

public record Schedule(Repeat Repeat, int? Times);

public record Alert(Reminder[] Reminders);

public enum Reminder
{
    AtEventTime, Before1Hour, Before30Min
}