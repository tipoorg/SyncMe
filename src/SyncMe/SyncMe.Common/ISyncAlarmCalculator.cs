using SyncMe.Models;

namespace SyncMe;

public interface ISyncAlarmCalculator
{
    bool TryGetNearestAlarm(SyncEvent syncEvent, out SyncAlarm syncALarm);
}
