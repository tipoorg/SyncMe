namespace SyncMe.Queries;

public record SyncEventQuery
{
    public int Month { get; init; }
    public int Year { get; init; }
}
