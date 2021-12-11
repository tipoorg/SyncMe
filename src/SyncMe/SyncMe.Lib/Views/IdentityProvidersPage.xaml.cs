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
    private readonly ISyncEventsService _syncEventsService;
    private readonly ISyncNamespaceRepository _syncNamespaceRepository;
    private readonly MicrosoftAuthorizationManager _authorizationManager;

    public string Image { get => AddOutlook.IsVisible ? "icon_arrow_major.png" : "icon_plus_minor.xml"; }

    public ObservableCollection<Identity> Identities { get; } = new ObservableCollection<Identity>();

    public IdentityProvidersPage(ISyncEventsService syncEventsService, ISyncNamespaceRepository syncNamespaceRepository, MicrosoftAuthorizationManager authorizationManager)
    {
        InitializeComponent();
        BindingContext = this;
        _syncEventsService = syncEventsService;
        _syncNamespaceRepository = syncNamespaceRepository;
        _authorizationManager = authorizationManager;

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
                if (!Identities.Any(i => i.Name == x.username) && !string.IsNullOrEmpty(x.username))
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
            _syncEventsService.RemoveEvents(e => username == e.ExternalEmail);

        var newEvents = syncEvents.Select(_syncEventsService.AddSyncEvent).ToList();

        return (username, newEvents);
    }

    private async Task LoadAllEventsAsync()
    {
        var accountsToResync = MicrosoftAuthorizationManager.CurrentAccounts.Select(a => a.Username).ToList();
        _syncEventsService.RemoveEvents(e => accountsToResync.Contains(e.ExternalEmail));
        foreach (var account in MicrosoftAuthorizationManager.CurrentAccounts)
        {
            var optional = await FetchEventsAsync(account.Username);

            if (!optional.HasValue)
                return;

            foreach (var @event in optional.Value.events)
            {
                _syncEventsService.AddSyncEvent(@event);
            }
        }
    }

    private async Task<Optional<(string username, IEnumerable<SyncEvent> events)>> FetchEventsAsync(string username)
    {
        var outlookNamespace = new Namespace { Key = "Outlook", IsActive = true };

        if (username is null)
        {
            var optional = await _authorizationManager.TrySignInAsync(App.AuthUIParent);

            if (!optional.HasValue)
                return default;

            username = optional.Value;
        }

        var client = await _authorizationManager.GetGraphClientAsync(username);
        var events = await new OutlookProvider(client, username).GetEventsAsync();

        _syncNamespaceRepository.TryAddSyncNamespace(outlookNamespace.Key);

        return (username, events.Select(e => e.ToSyncEvent(username))
                                .Select(e => e with { NamespaceKey = outlookNamespace.Key }));
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
