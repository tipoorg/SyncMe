namespace SyncMe.Services;

public interface IAlarmPlayer<TContext>
{
    void PlayAlarm(TContext context);
    void StopPlaying();
}
