using SyncMe.Models;

namespace SyncMe.Extensions;

public static class SyncEventExtensions
{
    public static SyncEvent TrimNamespaceEnd(this SyncEvent syncEvent)
    {
        syncEvent.Namespace.Title = syncEvent.Namespace.Title.TrimEnd('.');
        return syncEvent;
    }
}
