﻿using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Microsoft.Identity.Client;
using SyncMe.ExceptionHandling;
using Xamarin.Forms.Platform.Android;

namespace SyncMe.Droid;

[Activity(Label = "SyncMe", Icon = "@mipmap/icon_syncme", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : FormsAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        var exceptionHandler = AndroidStarter.GetService<IExceptionHandler>();
        TaskScheduler.UnobservedTaskException += exceptionHandler.TaskSchedulerOnUnobservedTaskException;
        AppDomain.CurrentDomain.UnhandledException += exceptionHandler.CurrentDomainOnUnhandledException;

        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        Xamarin.Forms.Forms.Init(this, savedInstanceState);

        var app = AndroidStarter.CreateApp();
        LoadApplication(app);

        App.AuthUIParent = this;
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper
            .SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}
