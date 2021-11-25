using Android.Content;

namespace SyncMe.Droid.Alarm;

public interface IAndroidAlarmPlayer
{
    void PlayAlarm(Context context);
    void StopPlaying();
}
