using SyncMe.Models;

namespace SyncMe.Services
{
    public interface ISyncAlarmService
    {
        bool TryGetNearestAlarm(Guid eventId, out SyncAlarm syncALarm);
    }
}