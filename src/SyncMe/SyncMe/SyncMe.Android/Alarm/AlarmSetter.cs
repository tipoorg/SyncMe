﻿using System;
using System.Text;
using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.Icu.Util;
using Android.OS;
using Android.Widget;

namespace SyncMe.Droid.Alarm;

internal class AlarmSetter : IAlarmSetter
{
    public void SetAlarm(int times, Context context)
    {
        var simpleDateFormat = new SimpleDateFormat("HH:mm:ss");
        var calendarItem = GetCalendarItem();
        var alarmIntent = GetAlarmIntent(times, context);

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
    private PendingIntent GetAlarmIntent(int times, Context context)
    {
        var intent = new Intent(context, typeof(AlarmReceiver));
        intent.PutExtra("TIMES", times);
        intent.AddFlags(ActivityFlags.IncludeStoppedPackages);
        intent.AddFlags(ActivityFlags.ReceiverForeground);
        int uniqueId = Guid.NewGuid().GetHashCode();
        return PendingIntent.GetBroadcast(context, uniqueId, intent, 0);
    }
}