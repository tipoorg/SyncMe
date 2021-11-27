using Android.Content;

namespace SyncMe.Droid.Alarm
{
    internal interface IAndroidAlarmService
    {
        void ProcessAlarm(Context context, Intent intent);

        void SetAlarm(Guid eventId, Context context);

        void StopPlayingAlarm(Intent intent);
    }
}