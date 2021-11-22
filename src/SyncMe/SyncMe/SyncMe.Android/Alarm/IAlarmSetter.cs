using Android.Content;

namespace SyncMe.Droid.Alarm;

public interface IAlarmSetter
{
    void SetAlarm(int times, Context context);
}
