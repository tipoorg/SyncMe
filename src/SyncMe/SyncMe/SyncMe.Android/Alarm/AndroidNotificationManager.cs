using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using SyncMe.Services;
using AndroidApp = Android.App.Application;

namespace SyncMe.Droid.Alarm;

internal class AndroidNotificationManager : INotificationManager<Context>
{
    private const string _channelId = "default";
    private const string _channelName = "Default";
    private const string _channelDescription = "The default channel for notifications.";

    private readonly NotificationManager _manager;

    private int _messageId = 0;
    private int _pendingIntentId = 0;

    public const string TitleKey = "title";
    public const string MessageKey = "message";

    public AndroidNotificationManager()
    {
        _manager = AndroidApp.Context.GetSystemService(Context.NotificationService) as NotificationManager;
        CreateNotificationChannel();
    }

    public void Show(string title, string message, Context context)
    {
        var notificationId = _messageId++;

        var notification = new NotificationCompat.Builder(context, _channelId)
            .SetContentIntent(GetShowNotificationIntent(title, message, context))
            .SetContentTitle(title)
            .SetContentText(message)
            .SetLargeIcon(BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.icon_about))
            .SetSmallIcon(Resource.Drawable.icon_about)
            .AddAction(Resource.Drawable.icon_feed, "STOP", GetStopNotificationIntent(context, notificationId))
            .SetAutoCancel(true)
            .Build();

        _manager.Notify(notificationId, notification);
    }

    public void Cancel(int notificationId)
    {
        _manager.Cancel(notificationId);
    }

    private PendingIntent GetShowNotificationIntent(string title, string message, Context context)
    {
        var intent = new Intent(context, typeof(MainActivity))
          .PutExtra(TitleKey, title)
          .PutExtra(MessageKey, message);

        return PendingIntent.GetActivity(context, _pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);
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
            var channel = new NotificationChannel(_channelId, channelNameJava, NotificationImportance.Default)
            {
                Description = _channelDescription
            };
            _manager.CreateNotificationChannel(channel);
        }
    }
}
