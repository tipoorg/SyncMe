﻿using SyncMe.Models;

namespace SyncMe.Extensions;

public static class SyncEventExtensions
{
    public static SyncEvent DecrementRemainingTimes(this SyncEvent syncEvent)
    {
        var remainingTimes = syncEvent.Schedule.Times - 1;
        return syncEvent with { Schedule = syncEvent.Schedule with { Times = remainingTimes } };
    }

    public static SyncEvent Activate(this SyncEvent syncEvent)
    {
        return syncEvent with { Status = SyncStatus.Active };
    }
}
