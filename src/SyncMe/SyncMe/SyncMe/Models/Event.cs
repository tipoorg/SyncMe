namespace SyncMe.Models;

public record Event(int Id, string Title, string Description, Namespace Namespace, Schedule Schedule, Alert Alert);

public record NamespaceTree(ILookup<Namespace, Namespace> Tree);

public record Namespace(int Id, string Title);

public enum Repeat
{
    Dayly, WeekDays
}

public record Schedule();

public record Alert();