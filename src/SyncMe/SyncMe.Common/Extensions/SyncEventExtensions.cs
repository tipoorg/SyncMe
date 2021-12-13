using SyncMe.Models;

namespace SyncMe.Extensions;

public static class SyncEventExtensions
{
    public static SyncEvent TrimNamespaceEnd(this SyncEvent syncEvent) =>
        syncEvent with { NamespaceKey = syncEvent.NamespaceKey.TrimEnd('.') };

    public static string GetParentNamespace(this SyncEvent syncEvent) =>
        syncEvent.NamespaceKey.Split(new char[] { '.' })[0];
}
