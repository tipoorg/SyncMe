using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal class AndroidNotificationManager
{
    private const string _channelId = "default";
    private const string _channelName = "Default";
    private const string _channelDescription = "The default channel for notifications.";

    private readonly NotificationManager _manager;

    private int _messageId = 0;
    public const string TitleKey = "title";

    private static AndroidNotificationManager _instance;
    public static AndroidNotificationManager Instance => _instance ??= new AndroidNotificationManager();

    private AndroidNotificationManager()
    {
        _manager = AndroidApp.Context.GetSystemService(Context.NotificationService) as NotificationManager;
        CreateNotificationChannel();
    }

    public void Show(string eventName, Context context)
    {
        var notificationId = _messageId++;
        var stopNotificationIntent = GetStopNotificationIntent(context, notificationId);

        var notification = new NotificationCompat.Builder(context, _channelId)
            .SetContentTitle(eventName)
            .SetContentText("OK")
            .SetContentIntent(stopNotificationIntent)
            .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.icon_open_calendar))
            .SetSmallIcon(Resource.Drawable.icon_open_calendar)
            .SetSilent(true)
            .SetAutoCancel(true)
            .Build();

        _manager.Notify(notificationId, notification);
    }

    public void Cancel(int notificationId)
    {
        _manager.Cancel(notificationId);
    }

    private PendingIntent GetStopNotificationIntent(Context context, int notificationId)
    {
        var intent = new Intent(context, typeof(AlarmReceiver))
            .PutExtra(AlarmMessage.NotificationIdKey, notificationId)
            .PutExtra(AlarmMessage.ActionKey, AlarmMessage.StopAlarmAction);

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
