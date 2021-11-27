using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Microsoft.Identity.Client;
using SyncMe.Droid.Alarm;
using Xamarin.Forms.Platform.Android;

namespace SyncMe.Droid;

[Activity(Label = "SyncMe", Icon = "@mipmap/SyncMeApp", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : FormsAppCompatActivity
{
    public const string Tag = "__Sync__Me__";

    private IAndroidAlarmService _androidAlarmService;
    private ISyncEventsRepository _syncEventsRepository;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        Xamarin.Forms.Forms.Init(this, savedInstanceState);

        var app = Bootstrapper.CreateApp();
        LoadApplication(app);

        _androidAlarmService = Bootstrapper.GetService<IAndroidAlarmService>();
        _syncEventsRepository = Bootstrapper.GetService<ISyncEventsRepository>();
        _syncEventsRepository.OnAddSyncEvent += OnAddSyncEvent;

        App.AuthUIParent = this;
    }

    private void OnAddSyncEvent(object sender, Guid eventId)
    {
        _androidAlarmService.SetAlarm(eventId, this);
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _syncEventsRepository.OnAddSyncEvent -= OnAddSyncEvent;

        base.Dispose(disposing);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper
            .SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
