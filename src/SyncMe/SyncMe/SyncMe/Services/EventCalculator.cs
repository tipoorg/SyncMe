using SyncMe.Models;

namespace SyncMe.Services;

public class EventCalculator
{
    public bool TryGetNearestAlarm(SyncEvent syncEvent, out TimeSpan delay)
    {
        delay = syncEvent switch
        {
            { Schedule.Repeat: SyncRepeat.Every10Seconds } => TimeSpan.FromSeconds(10),
            _ => throw new NotImplementedException()
        };

        return syncEvent.Schedule.Times is null or >0;
    }
}
