using Android.Content;

namespace SyncMe.Droid.Alarm
{
    internal interface IAndroidAlarmProcessor
    {
        void ProcessAlarm(Context context, Intent intent);

        void StopPlayingAlarm(Intent intent);
    }
}