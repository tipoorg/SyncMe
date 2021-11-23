using Microsoft.Extensions.DependencyInjection;
using GraphServiceClient = Microsoft.Graph.GraphServiceClient;
using MsGraph = Microsoft.Graph;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Net.Http.Headers;

namespace SyncMe;
public static class OAuthSettings
{
    public const string ApplicationId = "904b52b5-a7ad-4ad5-b7e5-23160e0800e1";
    public const string Scopes = "Calendars.Read";
    public const string RedirectUri = @"msauth://com.companyname.SyncMe";
}

public partial class App : Application, INotifyPropertyChanged
{
    private static IServiceProvider _serviceProvider;

    public static string[] Scopes = new[] { "Calendars.Read" };

    // UIParent used by Android version of the app
    public static object AuthUIParent = null;

    // Keychain security group used by iOS version of the app
    public static string iOSKeychainSecurityGroup = null;

    // Microsoft Authentication client for native/mobile apps
    public static IPublicClientApplication PCA;

    // Microsoft Graph client
    public static GraphServiceClient GraphClient;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        var builder = PublicClientApplicationBuilder
        .Create(OAuthSettings.ApplicationId)
        .WithRedirectUri(OAuthSettings.RedirectUri);

        if (!string.IsNullOrEmpty(iOSKeychainSecurityGroup))
        {
            builder = builder.WithIosKeychainSecurityGroup(iOSKeychainSecurityGroup);
        }

        PCA = builder.Build();

        MainPage = serviceProvider.GetRequiredService<AppShell>();
        _serviceProvider = serviceProvider;
    }

    public static T GetRequiredService<T>() => _serviceProvider.GetRequiredService<T>();
    public static Lazy<T> GetLazyRequiredService<T>() => new(() => GetRequiredService<T>());

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }

    //REFACTOR IT
    // Is a user signed in?
    private bool isSignedIn;
    public bool IsSignedIn
    {
        get { return isSignedIn; }
        set
        {
            isSignedIn = value;
            OnPropertyChanged("IsSignedIn");
            OnPropertyChanged("IsSignedOut");
        }
    }

    public bool IsSignedOut { get { return !isSignedIn; } }

    // The user's display name
    private string userName;
    public string UserName
    {
        get { return userName; }
        set
        {
            userName = value;
            OnPropertyChanged("UserName");
        }
    }

    // The user's email address
    private string userEmail;
    public string UserEmail
    {
        get { return userEmail; }
        set
        {
            userEmail = value;
            OnPropertyChanged("UserEmail");
        }
    }

    // The user's profile photo
    private ImageSource userPhoto;
    public ImageSource UserPhoto
    {
        get { return userPhoto; }
        set
        {
            userPhoto = value;
            OnPropertyChanged("UserPhoto");
        }
    }

    // The user's time zone
    public static TimeZoneInfo UserTimeZone;

    public async Task SignIn()
    {
        // First, attempt silent sign in
        // If the user's information is already in the app's cache,
        // they won't have to sign in again.
        try
        {
            var accounts = await PCA.GetAccountsAsync();

            var silentAuthResult = await PCA
                .AcquireTokenSilent(Scopes, accounts.FirstOrDefault())
                .ExecuteAsync();

        }
        catch (MsalUiRequiredException msalEx)
        {
            // This exception is thrown when an interactive sign-in is required.
            // Prompt the user to sign-in
            var interactiveRequest = PCA.AcquireTokenInteractive(Scopes);

            if (AuthUIParent != null)
            {
                interactiveRequest = interactiveRequest
                    .WithParentActivityOrWindow(AuthUIParent);
            }

            var interactiveAuthResult = await interactiveRequest.ExecuteAsync();
        }
        catch (Exception ex)
        {
        }

        await InitializeGraphClientAsync();
    }

    public async Task SignOut()
    {
        UserPhoto = null;
        UserName = string.Empty;
        UserEmail = string.Empty;
        IsSignedIn = false;
    }

    private async Task GetUserInfo()
    {
        UserPhoto = ImageSource.FromStream(() => GetUserPhoto());
        UserName = "Adele Vance";
        UserEmail = "adelev@contoso.com";
    }

    private Stream GetUserPhoto()
    {
        // Return the default photo
        return Assembly.GetExecutingAssembly().GetManifestResourceStream("GraphTutorial.no-profile-pic.png");
    }

    private async Task InitializeGraphClientAsync()
    {
        var currentAccounts = await PCA.GetAccountsAsync();
        try
        {
            if (currentAccounts.Count() > 0)
            {
                // Initialize Graph client
                GraphClient = new GraphServiceClient(new MsGraph.DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        var result = await PCA.AcquireTokenSilent(Scopes, currentAccounts.FirstOrDefault())
                            .ExecuteAsync();

                        requestMessage.Headers.Authorization =
                            new AuthenticationHeaderValue("Bearer", result.AccessToken);
                    }));

                await GetUserInfo();

                IsSignedIn = true;
            }
            else
            {
                IsSignedIn = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
}
