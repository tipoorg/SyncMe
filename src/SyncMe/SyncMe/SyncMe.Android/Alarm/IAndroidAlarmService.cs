using Android.Content;

namespace SyncMe.Droid.Alarm
{
    internal interface IAndroidAlarmService
    {
        void SetAlarm(Guid eventId, Context context);
    }
}