namespace SyncMe.Models;

public record SyncEvent
{
    public Guid Id { get; init; }
    public string? ExternalId { get; init; }
    public string? ExternalEmail { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string NamespaceKey { get; init; } = string.Empty;
    public SyncRepeat Repeat { get; init; }
    public SyncReminder Reminder { get; init; }
    public SyncStatus Status { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
}
