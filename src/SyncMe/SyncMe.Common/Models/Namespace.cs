namespace SyncMe.Models;

public record Namespace
{
    public static Namespace Root { get; } = new() { Key = "Root" };

    public string Key { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? TurnOnDate { get; set; }

    public string GetTitle() => Key.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
}
