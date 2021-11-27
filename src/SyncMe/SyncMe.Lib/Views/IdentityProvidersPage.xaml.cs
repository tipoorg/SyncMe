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
    private readonly ISyncEventsRepository _syncEventsRepository;
    private readonly ISyncNamespaceRepository _syncNamespaceRepository;

    public string Image { get => AddOutlook.IsVisible ? "icon_arrow_major.png" : "icon_plus_minor.xml"; }

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage(ISyncEventsRepository syncEventsRepository, ISyncNamespaceRepository syncNamespaceRepository)
    {
        InitializeComponent();
        BindingContext = this;
        _syncEventsRepository = syncEventsRepository;
        _syncNamespaceRepository = syncNamespaceRepository;

        var manager = new MicrosoftAuthorizationManager();

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
            .SelectMany(_ => LoadEventsAsync())
            .ObserveOn(SynchronizationContext.Current)
            .Subscribe(x =>
            {
                if (!Identities.Any(i => i.Name == x.username))
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
        var outlookNamespace = new Namespace { Title = "Outlook", IsActive = true };
        var manager = new MicrosoftAuthorizationManager();

        if (username is null)
        {
            var optional = await manager.TrySignInAsync(App.AuthUIParent);

            if (!optional.HasValue)
                return default;

            username = optional.Value;
        }

        var client = await manager.GetGraphClientAsync(username);
        var events = await new OutlookProvider(client, username).GetEventsAsync();

        if (!_syncNamespaceRepository.GetAllSyncNamespaces().Any(n => n.Value.Title == outlookNamespace.Title))
        {
            _syncNamespaceRepository.AddSyncNamespace(outlookNamespace.Title);
        }

        return (username, events.Select(e => e.ToSyncEvent(username))
                                .Select(e => e with { Namespace = outlookNamespace }));
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
