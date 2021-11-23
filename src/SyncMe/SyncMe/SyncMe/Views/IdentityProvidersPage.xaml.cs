using CalendarProviders.Authorization;
using SyncMe.Providers.OutlookProvider;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;
public record Identity(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentity;

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage()
    {
        InitializeComponent();
        BindingContext = this;

        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x =>
            {
                SwitchLayouts();
            });

        _addOutlookIdentity = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .SelectMany(async x =>
            {
                var manager = new MicrosoftAuthorizationManager();
                await manager.SignInAsync(App.AuthUIParent);
                var client = await manager.GetGraphClientAsync();
                var events = await new OutlookProvider(client, manager.CurrentAccounts.First().Username).GetEventsAsync();

                return (events, manager);
            }) // open browser and await task
            .Subscribe(x =>
            {
                Device.BeginInvokeOnMainThread(() => SwitchLayouts());
                Identities.Add(new Identity(x.manager.CurrentAccounts.First().Username));
            });
    }

    private void SwitchLayouts()
    {
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentity.Dispose();
    }
}
