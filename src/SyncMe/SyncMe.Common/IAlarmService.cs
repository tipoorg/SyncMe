using SyncMe.Models;

namespace SyncMe;

public interface IAlarmService
{
    void SetAlarmForEvent(SyncEvent syncEvent);
}
