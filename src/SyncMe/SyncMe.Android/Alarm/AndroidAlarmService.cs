using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;
using SyncMe.Droid.Extensions;
using SyncMe.Models;

namespace SyncMe.Droid.Alarm;

internal class AndroidAlarmService : IAndroidAlarmService
{
    private readonly ISyncAlarmService _syncAlarmService;
    private readonly IAndroidAlarmPlayer _androidAlarmPlayer;
    private readonly ISyncNamespaceService _syncNamespaceService;

    public AndroidAlarmService(
        ISyncAlarmService syncAlarmService,
        IAndroidAlarmPlayer androidAlarmPlayer,
        ISyncNamespaceService syncNamespaceService)
    {
        _syncAlarmService = syncAlarmService;
        _androidAlarmPlayer = androidAlarmPlayer;
        _syncNamespaceService = syncNamespaceService;
    }

    public void ProcessAlarm(Context context, Intent intent)
    {
        var pendingAlarm = intent.GetExtra<SyncAlarm>();
        if (_syncNamespaceService.IsNamespaceActive(pendingAlarm.NamespaceFullName))
        {
            _androidAlarmPlayer.PlayAlarm(context);
            AndroidNotificationManager.Instance.Show(pendingAlarm, context);
        }

        SetAlarm(pendingAlarm.EventId, context);
    }

    public void SetAlarm(Guid eventId, Context context)
    {
        if (_syncAlarmService.TryGetNearestAlarm(eventId, out var syncAlarm))
        {
            var calendarItem = GetCalendarItem(syncAlarm);
            var alarmIntent = GetAlarmIntent(syncAlarm, context);

            SetAlarm(calendarItem, alarmIntent, context);
            Toast.MakeText(
                context,
                $"{syncAlarm.Title} Scheduled on {DateTime.Now.AddSeconds(syncAlarm.DelaySeconds)}",
                ToastLength.Long).Show();
        }
    }

    public void StopPlayingAlarm(Intent intent)
    {
        _androidAlarmPlayer.StopPlaying();
        var id = intent.GetIntExtra(MessageKeys.NotificationIdKey, -1);
        AndroidNotificationManager.Instance.Cancel(id);
    }

    private static Calendar GetCalendarItem(SyncAlarm syncAlarm)
    {
        var calendarItem = Calendar.Instance;
        calendarItem.Add(CalendarField.Second, syncAlarm.DelaySeconds);
        return calendarItem;
    }

    private void SetAlarm(Calendar calendarItem, PendingIntent alarmIntent, Context context)
    {
        var am = context.GetSystemService(Context.AlarmService) as AlarmManager;
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            am.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, calendarItem.TimeInMillis, alarmIntent);
        else
            am.SetExact(AlarmType.RtcWakeup, calendarItem.TimeInMillis, alarmIntent);
    }

    private PendingIntent GetAlarmIntent(SyncAlarm syncAlarm, Context context)
    {
        var intent = new Intent(context, typeof(AlarmReceiver))
            .PutExtra(MessageKeys.ActionKey, MessageKeys.ProcessAlarmAction)
            .PutExtra(syncAlarm)
            .AddFlags(ActivityFlags.IncludeStoppedPackages)
            .AddFlags(ActivityFlags.ReceiverForeground);

        int uniqueId = Guid.NewGuid().GetHashCode();
        return PendingIntent.GetBroadcast(context, uniqueId, intent, PendingIntentFlags.Immutable);
    }
}
