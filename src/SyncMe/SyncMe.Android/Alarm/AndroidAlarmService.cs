using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;
using SyncMe.Droid.Extensions;
using SyncMe.Models;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal class AndroidAlarmService : IAlarmService
{
    public void SetAlarm(SyncAlarm syncAlarm)
    {
        var calendarItem = GetCalendarItem(syncAlarm);
        var alarmIntent = GetAlarmIntent(syncAlarm, AndroidApp.Context);

        SetAlarm(calendarItem, alarmIntent, AndroidApp.Context);
        Toast.MakeText(
            AndroidApp.Context,
            $"{syncAlarm.Title} Scheduled on {DateTime.Now.AddSeconds(syncAlarm.DelaySeconds)}",
            ToastLength.Long).Show();
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
