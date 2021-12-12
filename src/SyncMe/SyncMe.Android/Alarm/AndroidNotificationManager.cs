using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using Microsoft.Extensions.Logging;
using SyncMe.Models;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal class AndroidNotificationManager : INotificationManager
{
    private const string _channelId = "default";
    private const string _channelName = "Default";
    private const string _channelDescription = "The default channel for notifications.";

    private readonly NotificationManager _manager;
    private readonly ILogger<AndroidNotificationManager> _logger;
    private int _messageId = 0;
    public const string TitleKey = "title";

    public AndroidNotificationManager(ILogger<AndroidNotificationManager> logger)
    {
        _logger = logger;
        _manager = AndroidApp.Context.GetSystemService(Context.NotificationService) as NotificationManager;
        CreateNotificationChannel();
    }

    public void Show(SyncAlarm syncAlarm)
    {
        var notificationId = _messageId++;
        var stopNotificationIntent = GetStopNotificationIntent(AndroidApp.Context, notificationId);

        var notification = new NotificationCompat.Builder(AndroidApp.Context, _channelId)
            .SetContentTitle(syncAlarm.Title)
            .SetContentText("OK")
            .SetContentIntent(stopNotificationIntent)
            .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Mipmap.icon_syncme))
            .SetSmallIcon(Resource.Mipmap.icon_syncme)
            .SetSilent(true)
            .SetAutoCancel(true)
            .Build();

        _manager.Notify(notificationId, notification);
        _logger.LogInformation($"{syncAlarm.Title} notified");
    }

    public void Cancel(int notificationId)
    {
        _manager.Cancel(notificationId);
    }

    private PendingIntent GetStopNotificationIntent(Context context, int notificationId)
    {
        var intent = new Intent(context, typeof(AlarmReceiver))
            .PutExtra(MessageKeys.NotificationIdKey, notificationId)
            .PutExtra(MessageKeys.ActionKey, MessageKeys.StopAlarmAction);

        int uniqueId = Guid.NewGuid().GetHashCode();

        return PendingIntent.GetBroadcast(context, uniqueId, intent, PendingIntentFlags.Immutable);
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channelNameJava = new Java.Lang.String(_channelName);
            var channel = new NotificationChannel(_channelId, channelNameJava, NotificationImportance.High)
            {
                Description = _channelDescription,
                LockscreenVisibility = NotificationVisibility.Public
            };

            channel.SetShowBadge(true);

            _manager.CreateNotificationChannel(channel);
        }
    }
}
