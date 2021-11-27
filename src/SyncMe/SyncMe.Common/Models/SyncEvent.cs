namespace SyncMe.Models;

public record SyncEvent
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
