namespace SyncMe.Services;

public interface IAlarmSetter<TContext>
{
    void SetAlarm(int times, TContext context);
}
