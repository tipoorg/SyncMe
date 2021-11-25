using SyncMe.Models;
using SyncMe.Repos;
using System.Threading;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms.Xaml;
using SyncMe.Extensions;
using SyncMe.Common;
using SyncMe.CalendarProviders.Authorization;
using SyncMe.CalendarProviders.Outlook;

namespace SyncMe.Views;

public record Identity(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentitySubscription;
    private readonly ISyncEventsRepository _syncEventsRepository;

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage(ISyncEventsRepository syncEventsRepository)
    {
        InitializeComponent();
        BindingContext = this;
        _syncEventsRepository = syncEventsRepository;

        var manager = new MicrosoftAuthorizationManager();

        foreach(var account in MicrosoftAuthorizationManager.CurrentAccounts)
        {
            Identities.Add(new Identity(account.Username));
        }

        _addIdentitySubsciption = Observable
            .FromEventPattern(AddIdentity, nameof(Button.Clicked))
            .Subscribe(x => SwitchLayouts());

        _addOutlookIdentitySubscription = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .Do(_ => SwitchLayouts())
            .SelectMany(_ => LoadEventsAsync())
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x =>
            {
                if(!Identities.Any(i => i.Name == x.username))
                    Identities.Add(new Identity(x.username));
            });
    }

    private async Task<(string username, List<Guid> newEvents)> LoadEventsAsync(string username = null)
    {
        var optional = await FetchEventsAsync(username);
        if (!optional.HasValue)
            return default;
        username = optional.Value.username;
        var syncEvents = optional.Value.events;

        if (Identities.Any(i => i.Name == username))
            _syncEventsRepository.RemoveEvents(e => username == e.ExternalEmail);

        var newEvents = syncEvents.Select(_syncEventsRepository.AddSyncEvent).ToList();

        return (username, newEvents);
    }

    private async Task LoadAllEventsAsync()
    {
        var manager = new MicrosoftAuthorizationManager();
        var accountsToResync = MicrosoftAuthorizationManager.CurrentAccounts.Select(a => a.Username).ToList();
        _syncEventsRepository.RemoveEvents(e => accountsToResync.Contains(e.ExternalEmail));
        foreach (var account in MicrosoftAuthorizationManager.CurrentAccounts)
        {
            var optional = await FetchEventsAsync(account.Username);

            if (!optional.HasValue)
                return;

            foreach (var @event in optional.Value.events)
            {
                _syncEventsRepository.AddSyncEvent(@event);
            }
        }
    }

    private async Task<Optional<(string username, IEnumerable<SyncEvent> events)>> FetchEventsAsync(string username)
    {
        var manager = new MicrosoftAuthorizationManager();
        var optional = await manager.TrySignInAsync(App.AuthUIParent);

        if (!optional.HasValue)
            return default;

        var client = await manager.GetGraphClientAsync(username);
        var events = await new OutlookProvider(client, username).GetEventsAsync();
        return (username, events.Select(e => e.ToSyncEvent(username)));
    }

    private void SwitchLayouts()
    {
        AddOutlook.IsVisible = !AddOutlook.IsVisible;
        AddGoogle.IsVisible = !AddGoogle.IsVisible;
    }

    private async void OnSyncClicked(object sender, EventArgs e)
    {
        if (sender is Button { CommandParameter: Identity selectedItem })
        {
            await LoadEventsAsync(selectedItem.Name);
        }
        else
        {
            await LoadAllEventsAsync();
        }
    }

    public void Dispose()
    {
        _addIdentitySubsciption.Dispose();
        _addOutlookIdentitySubscription.Dispose();
    }
}
