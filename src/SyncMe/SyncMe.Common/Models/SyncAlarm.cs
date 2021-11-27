namespace SyncMe.Models;

public record SyncAlarm(string Title, int EventId, string NamespaceFullName, int DelaySeconds);
