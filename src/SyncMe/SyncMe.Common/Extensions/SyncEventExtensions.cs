using SyncMe.Models;

namespace SyncMe.Extensions;

public static class SyncEventExtensions
{
    public static SyncEvent TrimNamespaceEnd(this SyncEvent syncEvent) =>
        syncEvent with { NamespaceKey = syncEvent.NamespaceKey.TrimEnd('.') };
}
