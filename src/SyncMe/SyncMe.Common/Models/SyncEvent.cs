namespace SyncMe.Models;

public record SyncEvent
{
    public int Id { get; set; }
    public string? ExternalId { get; set; }
    public string? ExternalEmail { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string NamespaceKey { get; set; } = string.Empty;
    public SyncRepeat Repeat { get; set; }
    public SyncReminder Reminder { get; set; }
    public SyncStatus Status { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
