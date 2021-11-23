namespace SyncMe.Models;

internal class EventModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
}
