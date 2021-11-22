using System.Text;
using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;
using SyncMe.Services;

namespace SyncMe.Droid.Alarm;

internal class AndroidAlarmSetter : IAlarmSetter<Context>
{
    public void SetAlarm(int times, Context context)
    {
        var simpleDateFormat = new SimpleDateFormat("HH:mm:ss");
        var calendarItem = GetCalendarItem();
        var alarmIntent = GetSetAlarmIntent(times, context);

        SetAlarm(calendarItem, alarmIntent, context);

        var textTimer = new StringBuilder("Alarm has been set")
            .AppendLine()
            .Append(simpleDateFormat.Format(calendarItem.TimeInMillis))
            .AppendLine();

        Toast.MakeText(context, textTimer.ToString(), ToastLength.Short).Show();
    }

    private static Calendar GetCalendarItem()
    {
        var calendarItem = Calendar.Instance;
        calendarItem.Add(CalendarField.Second, 5);
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
    private PendingIntent GetSetAlarmIntent(int times, Context context)
    {
        var intent = new Intent(context, typeof(AlarmReceiver))
            .PutExtra(AlarmMessage.ActionKey, AlarmMessage.SetAlarmAction)
            .PutExtra(AlarmMessage.TimesKey, times)
            .AddFlags(ActivityFlags.IncludeStoppedPackages)
            .AddFlags(ActivityFlags.ReceiverForeground);

        int uniqueId = Guid.NewGuid().GetHashCode();
        return PendingIntent.GetBroadcast(context, uniqueId, intent, PendingIntentFlags.Immutable);
    }
}
