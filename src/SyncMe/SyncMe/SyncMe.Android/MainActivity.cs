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

[Activity(Label = "SyncMe", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : FormsAppCompatActivity
{
    private IDisposable _setAlarmSubscription;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        Xamarin.Forms.Forms.Init(this, savedInstanceState);

        var app = Bootstrapper.CreateApp();
        LoadApplication(app);

        App.AuthUIParent = this;

        _setAlarmSubscription = App.GetRequiredService<NotesPage>().SetAlarmClicks
            .Subscribe(x => App.GetRequiredService<IAlarmSetter>().SetAlarm(x, this));
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
