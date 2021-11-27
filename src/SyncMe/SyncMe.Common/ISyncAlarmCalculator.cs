using SyncMe.Models;

namespace SyncMe;

public interface ISyncAlarmCalculator
{
    bool TryGetNearestAlarm(Guid eventId, out SyncAlarm syncALarm);
}
