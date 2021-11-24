using System.Reactive.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Microsoft.Identity.Client;
using SyncMe.Droid.Alarm;
using SyncMe.Views;
using Xamarin.Forms.Platform.Android;

namespace SyncMe.Droid;
[Activity(Label = "SyncMe", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : FormsAppCompatActivity
{
    private IDisposable _setAlarmSubscription;
    private IAndroidAlarmService _androidAlarmService;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        Xamarin.Forms.Forms.Init(this, savedInstanceState);

        var app = Bootstrapper.CreateApp();
        LoadApplication(app);

        _androidAlarmService = Bootstrapper.GetService<IAndroidAlarmService>();

        _setAlarmSubscription = Bootstrapper.GetService<CreateEventPage>().ScheduledEvents
            .Merge(Bootstrapper.GetService<IdentityProvidersPage>().ScheduledEvents)
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x => _androidAlarmService.SetAlarm(x, this));
        App.AuthUIParent = this;
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            _setAlarmSubscription.Dispose();

        base.Dispose(disposing);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper
            .SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
