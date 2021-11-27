using SyncMe.Models;

namespace SyncMe;

public interface IAlarmService
{
    void SetAlarm(SyncAlarm syncAlarm);
}
