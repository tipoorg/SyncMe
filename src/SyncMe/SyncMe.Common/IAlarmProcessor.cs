using SyncMe.Models;

namespace SyncMe;

public interface IAlarmProcessor
{
    void ProcessAlarm(SyncAlarm pendingAlarm);
    void StopPlayingAlarm(int notificationId);
}
