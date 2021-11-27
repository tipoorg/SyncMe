namespace SyncMe.Models;

public record Namespace
{
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? TurnOnDate { get; set; }
}
