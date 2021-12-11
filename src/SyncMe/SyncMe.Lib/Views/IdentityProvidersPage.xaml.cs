using System.Collections.ObjectModel;
using System.Reactive.Linq;
using SyncMe.CalendarProviders.Authorization;
using SyncMe.CalendarProviders.Extensions;
using SyncMe.CalendarProviders.Outlook;
using SyncMe.Functional;
using SyncMe.Models;
using Xamarin.Forms.Xaml;

namespace SyncMe.Views;

public record Identity(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentitySubscription;
    private readonly IIdentitiesService _identitiesService;

    public string Image { get => AddOutlook.IsVisible ? "icon_arrow_major.png" : "icon_plus_minor.xml"; }

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage(IIdentitiesService identitiesService)
    {
        InitializeComponent();
        BindingContext = this;
        _identitiesService=identitiesService;

        foreach (var account in MicrosoftAuthorizationManager.CurrentAccounts)
        {
            Identities.Add(new Identity(account.Username));
        }

        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x => SwitchLayouts());

        _addOutlookIdentitySubscription = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .Do(_ => SwitchLayouts())
            .SelectMany(_ => _identitiesService.AddNewIdentity())
            .Where(x => x.Filter(username => !Identities.Any(i => i.Name == username)).IsSome)
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x => x.Do(username => Identities.Add(new Identity(username))));
    }

    private void SwitchLayouts()
    {
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
        OnPropertyChanged(nameof(Image));
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: Identity selectedItem })
        {
            await _identitiesService.LoadEventsAsync(selectedItem.Name);
        }
        else
        {
            foreach (var account in MicrosoftAuthorizationManager.CurrentAccounts)
                await _identitiesService.LoadEventsAsync(account.Username);
        }
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentitySubscription.Dispose();
    }
}
