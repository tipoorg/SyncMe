using SyncMe.Models;

namespace SyncMe;

public interface ISyncAlarmCalculator
{
    bool TryGetNearestAlarm(int eventId, out SyncAlarm syncALarm);
}
