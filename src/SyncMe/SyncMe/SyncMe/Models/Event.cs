namespace SyncMe.Models;

public enum Status
{
    Active, Stopped
}

public record Event(int Id, string Title, string Description, Namespace Namespace, Schedule Schedule, Alert Alert, Status Status);

public record NamespaceTree(ILookup<Namespace, Namespace> Tree);

public record Namespace(int Id, string Title);

public enum Repeat
{
    Dayly, WeekDays, EveryMonth, EveryYear
}

public record Schedule(Repeat Repeat);

public record Alert(Reminder[] Reminders);

public enum Reminder
{
    AtEventTime, Before1Hour, Before30Min
}