using System;
using System.Threading.Tasks;
using Foundation;
using Microsoft.Identity.Client;
using SyncMe.ExceptionHandling;
using UIKit;

namespace SyncMe.IOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
{
    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
        var exceptionHandler = IOSStarter.GetService<IExceptionHandler>();

        TaskScheduler.UnobservedTaskException += exceptionHandler.TaskSchedulerOnUnobservedTaskException;
        AppDomain.CurrentDomain.UnhandledException += exceptionHandler.CurrentDomainOnUnhandledException;

        Xamarin.Forms.Forms.Init();
        LoadApplication(IOSStarter.CreateApp());

        return base.FinishedLaunching(app, options);
    }

    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
        return true;
    }
}
