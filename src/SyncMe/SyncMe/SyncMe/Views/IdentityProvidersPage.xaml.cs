using CalendarProviders.Authorization;
using SyncMe.Models;
using SyncMe.Providers.OutlookProvider;
using SyncMe.Repos;
using System.Threading;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Xamarin.Forms.Xaml;
using SyncMe.Extensions;

namespace SyncMe.Views;
public record Identity(string Name);

[XamlCompilation(XamlCompilationOptions.Compile)]
public sealed partial class IdentityProvidersPage : ContentPage, IDisposable
{
    private readonly IDisposable _addIdentitySubsciption;
    private readonly IDisposable _addOutlookIdentity;
    private readonly IDisposable _addGoogleIdentity;
    private readonly IDisposable _addEventConnection;
    private readonly ISyncEventsRepository _syncEventsRepository;

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();
    public IObservable<Guid> ScheduledEvents { get; }

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
            .Subscribe(x =>
            {
                SwitchLayouts();
            });

        var scheduledEvents = Observable
            .FromEventPattern(AddOutlook, nameof(Button.Clicked))
            .Do(_ => SwitchLayouts())
            .SelectMany(_ => LoadEventsAsync())
            .Publish();

        _addEventConnection = scheduledEvents.Connect();
        ScheduledEvents = scheduledEvents.SelectMany(x => x.newEvents);
        _addOutlookIdentity = scheduledEvents
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x =>
            {
                Device.BeginInvokeOnMainThread(() => SwitchLayouts());

                if(!Identities.Any(i => i.Name == x.username))
                    Identities.Add(new Identity(x.username));
            });

        _addGoogleIdentity = Observable
            .FromEventPattern(AddGoogle, nameof(Button.Clicked))
            .SelectMany(async x =>
            {
                var manager = new MicrosoftAuthorizationManager();
                var accountsToResync = MicrosoftAuthorizationManager.CurrentAccounts.Select(a => a.Username).ToList();
                syncEventsRepository.RemoveEvents(e => accountsToResync.Contains(e.ExternalEmail));
                foreach (var account in MicrosoftAuthorizationManager.CurrentAccounts)
                {
                    var username = account.Username;
                    var client = await manager.GetGraphClientAsync(account.Username);
                    var events = await new OutlookProvider(client, username).GetEventsAsync();

                    var syncEvents = events.Select(e => e.ToSyncEvent(username));

                    foreach (var @event in syncEvents)
                    {
                        syncEventsRepository.AddSyncEvent(@event);
                    }
                }

                return manager;
            }) // open browser and await task
            .Subscribe(x =>
            {
                Device.BeginInvokeOnMainThread(() => SwitchLayouts());
            });
    }

    private async Task<(string username, List<Guid> newEvents)> LoadEventsAsync(string username = null)
    {
        var syncEvents = await FetchEventsAsync(username);

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
            var syncEvents = await FetchEventsAsync(account.Username);

            foreach (var @event in syncEvents)
            {
                _syncEventsRepository.AddSyncEvent(@event);
            }
        }
    }

    private async Task<IEnumerable<SyncEvent>> FetchEventsAsync(string username)
    {
        var manager = new MicrosoftAuthorizationManager();
        username ??= await manager.SignInAsync(App.AuthUIParent);
        var client = await manager.GetGraphClientAsync(username);
        var events = await new OutlookProvider(client, username).GetEventsAsync();
        return events.Select(e => e.ToSyncEvent(username));
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
        _addOutlookIdentity.Dispose();
        _addEventConnection.Dispose();
    }
}
