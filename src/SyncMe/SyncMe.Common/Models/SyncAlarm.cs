namespace SyncMe.Models;

public record SyncAlarm(string Title, Guid EventId, string NamespaceFullName, int DelaySeconds);
