using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms.Platform.Android;

namespace SyncMe.Droid;

[Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
public class SplashActivity : FormsAppCompatActivity
{
    public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
    {
        base.OnCreate(savedInstanceState, persistentState);
    }

    protected override void OnResume()
    {
        try
        {
            base.OnResume();
            var intent = new Intent(this, typeof(MainActivity));
            if (Intent.Extras != null)
            {
                intent.PutExtras(Intent.Extras);
            }
            StartActivity(intent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}
