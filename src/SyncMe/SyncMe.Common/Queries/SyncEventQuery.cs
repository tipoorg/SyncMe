namespace SyncMe.Queries;

public record SyncEventQuery
{
    public int? StartMonth { get; init; }
    public int? StartYear { get; init; }
}
