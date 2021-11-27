using SyncMe.Models;

namespace SyncMe;

public interface ISyncAlarmService
{
    bool TryGetNearestAlarm(Guid eventId, out SyncAlarm syncALarm);
}
