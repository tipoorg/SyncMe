using EventKit;
using Foundation;
using GlobalToast;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using UIKit;
using UserNotifications;

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
        Xamarin.Forms.Forms.Init();
        LoadApplication(IOSStarter.CreateApp());

        RequestRequiredAccess();

        return base.FinishedLaunching(app, options);
    }

    private static void RequestRequiredAccess()
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 14))
        {
            IOSStarter.GetService<ILogger<AppDelegate>>().LogInformation("About to start asking required permissions.");
            RequestNotificationsPermissions();
            RequestEventStorePermissions();
        }
    }

    private static void RequestEventStorePermissions()
    {
        IOSStarter.GetService<EKEventStore>().RequestAccess(EKEntityType.Event, (granted, error) =>
        {
            if (!granted)
            {
                IOSStarter.GetService<ILogger<AppDelegate>>().LogError($"Calendar permission was not granted. Error: {error?.Description}");
                Toast.MakeToast("We need to have Calendar access before launching app.").Show();
            }
            else
            {
                IOSStarter.GetService<ILogger<AppDelegate>>().LogInformation("Calendar permission is granted.");
            }
        });
    }

    private static void RequestNotificationsPermissions()
    {
        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert
                                                            | UNAuthorizationOptions.Sound
                                                            | UNAuthorizationOptions.Badge, (granted, error) =>
        {
            if (!granted)
            {
                IOSStarter.GetService<ILogger<AppDelegate>>().LogError($"Notitification permission was not granted. Error: {error?.Description}");
                Toast.MakeToast("We need to have Notifications access before launching app.").Show();
            }
            else
            {
                IOSStarter.GetService<ILogger<AppDelegate>>().LogInformation("Notifications permission is granted.");
            }
        });
    }

    public override void FinishedLaunching(UIApplication application)
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(10, 14))
            //NSProcessInfo.ProcessInfo.IsOperatingSystemAtLeastVersion(new NSOperatingSystemVersion(10, 14, 0)))
        {
            RequestNotificationsPermissions();
        }
    }

    public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
    {
        // show an alert
        var okayAlertController = UIAlertController.Create(notification.AlertAction, notification.AlertBody, UIAlertControllerStyle.Alert);
        okayAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

        Window.RootViewController.PresentViewController(okayAlertController, true, null);

        // reset our badge
        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
    }

    public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
    {
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(url);
        return true;
    }
}
