using Android.App;
using Android.Content;
using Android.Icu.Util;
using Android.OS;
using SyncMe.Droid.Extensions;
using SyncMe.Models;
using SyncMe.Services;

namespace SyncMe.Droid.Alarm;

internal class AndroidAlarmIntent
{
    public void SetAlarm(SyncEvent syncEvent, Context context)
    {
        if (new EventCalculator().TryGetNearestAlarm(syncEvent, out var delay))
        {
            var calendarItem = GetCalendarItem(delay);
            var alarmIntent = GetAlarmIntent(syncEvent, context);

            SetAlarm(calendarItem, alarmIntent, context);
        }
    }

    private static Calendar GetCalendarItem(TimeSpan delay)
    {
        var calendarItem = Calendar.Instance;
        calendarItem.Add(CalendarField.Second, (int)delay.TotalSeconds);
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

    private PendingIntent GetAlarmIntent(SyncEvent syncEvent, Context context)
    {
        var intent = new Intent(context, typeof(AlarmReceiver))
            .PutExtra(AlarmMessage.ActionKey, AlarmMessage.SetAlarmAction)
            .PutExtra(syncEvent)
            .AddFlags(ActivityFlags.IncludeStoppedPackages)
            .AddFlags(ActivityFlags.ReceiverForeground);

        int uniqueId = Guid.NewGuid().GetHashCode();
        return PendingIntent.GetBroadcast(context, uniqueId, intent, PendingIntentFlags.Immutable);
    }
}
